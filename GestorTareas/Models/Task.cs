using System;
using System.Data.Common;
using System.Diagnostics.Tracing;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using GestorTareas.Enums;
using GestorTareas.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SimpleTask), "SimpleTask")]
[JsonDerivedType(typeof(RecurringTask), "RecurringTask")]
[JsonDerivedType(typeof(CompositeTask), "CompositeTask")]
[JsonDerivedType(typeof(SubTask), "SubTask")]

public abstract class Task : IIdentificable
{
    public int Id { get; set; }
    public string Title
    {
        get; set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El título no puede estar vacío");
            if (value.Length > 30)
                throw new ArgumentException("El título no puede contener más de 30 caracteres");
            field = value.Trim();
        }
    }
    public string? TaskDescription
    {
        get; set
        {
            if (string.IsNullOrWhiteSpace(value))
                field = null;
            else if (value.Length > 300)
                throw new ArgumentException("LA descripción no puede ser superior a 300 caracteres.");
            else
                field = value.Trim();
        }
    }
    public TaskType TaskType { get; set; } = TaskType.SimpleTask;
    private Priority _priority = Priority.Normal;
    public Priority TaskPriority
    {
        get => _priority;
        set
        {
            if (!Enum.IsDefined(typeof(Priority), value))
                throw new ArgumentException("La prioridad NO es válida.");

            _priority = value;
        }
    }

    private TaskStatus _status =TaskStatus.Pending;
    public TaskStatus TaskStatus
    {
        get => _status;
        set
        {
            if (!Enum.IsDefined(typeof(TaskStatus), value))
                throw new ArgumentException("El estado no es válido.");
            _status = value;
        }
    }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    private DateTime? _updatedAt;
    public DateTime? UpdatedAt
    {
        get => _updatedAt;
        set
        {
            if (value.HasValue && value < CreatedAt)
                throw new ArgumentException("La fecha actualización NO puede ser menor a la fecha de creación.");
            _updatedAt = value;
        }
    }

    public DateTime? DueTime { get; set; } = null;
    private string? _cancelReason = null;
    public string? CancelReason
    {
        get => _cancelReason;
        set
        {
            if (string.IsNullOrEmpty(value))
                value = "Motivo Cancelación Sin Determinar";//TODO: comprobar esta asignacion
            if (value.Length > 250)
                throw new ArgumentException("La longitud del valor no puede ser mayor de 250 caracteres.");
            _cancelReason = value;
        }
    }

    public int UserId { get; set; }
    public User? User { get; set; }
    public ICollection<LinkedTask> Dependencies { get; set; } = new List<LinkedTask>(10);
    public ICollection<LinkedTask> RequiredByOtherTask { get; set; } = new List<LinkedTask>(10);

    // Constructor vacio para trabajar la serialización
    // con polimorfismo
    [JsonConstructor]
    protected Task() : base() { }
    protected Task(
        string title,
        int userId,
        string? taskDescription = null,
        TaskType taskType = TaskType.SimpleTask,
        Priority taskPriority = Priority.Normal,
        TaskStatus taskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        string? cancelReason = null)
    {
        Title = title.Trim();
        UserId = userId;
        TaskDescription = taskDescription?.Trim() ?? "Sin descripción.";
        TaskType = taskType;
        TaskPriority = taskPriority;
        TaskStatus = taskStatus;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        DueTime = dueTime;
        CancelReason = cancelReason ?? $"Tarea no cancelada. Estado: {this.TaskStatus.ToString()}";
    }

    public void RenameTask(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Título vacío");
        if (newTitle.Length > 30)
            throw new ArgumentException("Máx 30 caracteres");
        Title = newTitle.Trim();
        AddUpdatedDate();
    }

    protected void AddUpdatedDate()
    {
        this.UpdatedAt=DateTime.UtcNow;
    }

    public void UpdateTaskDescription(string newTaskDescription)
    {
        if (string.IsNullOrWhiteSpace(newTaskDescription))
            newTaskDescription = "Sin descripcion.";
        if (newTaskDescription.Length > 300)
            throw new ArgumentException("LA descripción no puede ser superior a 300 caracteres.");
        this.TaskDescription = newTaskDescription;
        AddUpdatedDate();
    }
    public void ChangePriority(Priority newTaskPriority)
    {
        this.TaskPriority = newTaskPriority;
        AddUpdatedDate();
    }
    public void UpdateDueTime(DateTime newDueTime)
    {
        ValidateDueTime(newDueTime, CreatedAt);
        DueTime = newDueTime;
        AddUpdatedDate();
    }

    private void ValidateDueTime(DateTime? newDueTime, DateTime? createdAt=null)
    {
        if (!newDueTime.HasValue)
            return;
        if (newDueTime > DateTime.UtcNow.AddYears(2))
            throw new ArgumentException("La fecha de fin de tarea es mayor a 2 años, No es una fecha válida.");
        if (newDueTime <= DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException("La fecha introducida para su vencimiento no puede ser anterior a la actual.");
        if (createdAt.HasValue && newDueTime < createdAt.Value)
            throw new ArgumentException("La fecha de vencimiento introducida no puede ser inferiior a la de creación.");
    }

    public bool CompleteTask()
    {
        if (TaskStatus != TaskStatus.Completed && TaskStatus != TaskStatus.Cancelled)
        {
            TaskStatus = TaskStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
            AddUpdatedDate();
            return true;
        }
        AddUpdatedDate();
        return false;
    }
    public void ReopenTask()
    {
        if (this.TaskStatus == TaskStatus.InProgress)
            return;
        this.TaskStatus = TaskStatus.InProgress;
        AddUpdatedDate();
    }
    public void CancelTask(string cancelReason)
    {
        if (this.TaskStatus != TaskStatus.Completed && this.TaskStatus != TaskStatus.Cancelled)
        {
            this.CancelReason = cancelReason ?? "No se aporta motivo.";
            this.TaskStatus = TaskStatus.Cancelled;
            AddUpdatedDate();
        }
        else
        {
            throw new Exception($"La tarea no se pudo Cancelar porque la tarea estaba {this.TaskStatus}");
        }
    }
    public void StartTask()
    {
        if (this.TaskStatus == TaskStatus.Pending)
        {
            this.TaskStatus = TaskStatus.InProgress;
            AddUpdatedDate();
        }
        else
        {
            throw new Exception($"La Tareas no se pudo iniciar porque la tarea está {this.TaskStatus}");
        }
    }
    public bool IsOverdue()
    {
        if (this.DueTime is null)
            return false;

        if (this.TaskStatus == TaskStatus.Completed || this.TaskStatus == TaskStatus.Cancelled)
            return false;

        return DateTime.UtcNow > this.DueTime;
    }
    public int CalculateOverDueDays()
    {
        if (!this.DueTime.HasValue)
            throw new InvalidOperationException("No existe fecha de fin establecida.");
        if (this.DueTime.Value.Date >= DateTime.UtcNow.Date)
            return 0;

        return (DateTime.UtcNow.Date - this.DueTime.Value.Date).Days;
    }
    public int CalculateRemainingDays()
    {
        if (!this.DueTime.HasValue)
            throw new InvalidOperationException("No existe fecha de fin establecida.");

        return Math.Max(0,(this.DueTime.Value.Date - DateTime.UtcNow.Date).Days);
    }
    public abstract string ResumeTask();

}

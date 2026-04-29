using System;
using System.Data.Common;
using System.Diagnostics.Tracing;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using GestorTareas.Enums;
using GestorTareas.Interfaces;
using TaskStatus = GestorTareas.Enums.TaskStatus;



namespace GestorTareas.Models;

//Para trabajar con polimorfismo y JSON

[System.Text.Json.Serialization.JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[System.Text.Json.Serialization.JsonDerivedType(typeof(SimpleTask), "SimpleTask")]
[System.Text.Json.Serialization.JsonDerivedType(typeof(RecurringTask), "RecurringTask")]
[System.Text.Json.Serialization.JsonDerivedType(typeof(CompositeTask), "CompositeTask")]
[System.Text.Json.Serialization.JsonDerivedType(typeof(SubTask), "SubTask")]
[System.Text.Json.Serialization.JsonDerivedType(typeof(LinkedTask), "LinkedTask")]

public abstract class Task : IIdentificable
{
    public Guid Id { get; set; }
    public string Title
    {
        get; set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El título no puede estar vacío");
            if (value.Length > 20)
                throw new ArgumentException("El título no puede contener más de 20 caracteres");
            field = value.Trim();
        }
    }

    public string? TaskDescription
    {
        get; set
        {
            if (string.IsNullOrWhiteSpace(value))
                field = "Sin descripcion.";
            else if (value.Length > 250)
                throw new ArgumentException("LA descripción no puede ser superior a 250 caracteres.");
            else
                field = value.Trim();
        }
    }

    public TaskPriority? Priority
    {
        get; set
        {
            if (value is not null && !Enum.IsDefined(typeof(TaskPriority), value))
                throw new ArgumentException("La prioridad NO es válida.");
            else if (value is null)
                field = TaskPriority.Normal;
            else
                field = value;
        }
    }

    private TaskStatus _status;
    public TaskStatus? Status
    {
        get; set
        {
            if (value is not null && !Enum.IsDefined(typeof(TaskStatus), value))
            {
                throw new ArgumentException("El estado no es válido.");
            }
            else if (value is null)
            {
                value = TaskStatus.Pending;
            }
            //TODO: comprobar está bien aplicada.

            else if (_status == TaskStatus.Completed && (value == TaskStatus.InProgress || value == TaskStatus.Pending))
            {
                throw new ArgumentException("No se puede modificar el estado de una tarea ya completada.");
            }
            _status = (TaskStatus)value;
        }
    }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt
    {
        get; set
        {
            if (value is null || value < CreatedAt)
                throw new ArgumentException("La fecha actualización NO puede ser menor a la fecha de creación.");
            field = value;
        }
    }

    public DateTime? DueTime
    {
        get;

        set
        {
            if (value is not null && value <= DateTime.Now)
                throw new ArgumentException("La fecha introducida para su vencimiento no puede ser anterior a la actual.");
            else if (value > DateTime.Now.AddYears(2))
                throw new ArgumentException("La fecha de fin de tarea es mayor a 2 años, No es una fecha válida.");
            field = value;
        }
    }
    public string? CancelReason { get; set; } = null;

    public Guid UserId { get; set; }
    public User User { get; set; }

    public List<User> UsersList { get; set; } = new(10);
    // Constructor vacio para trabajar la serialización
    // con polimorfismo
    [JsonConstructor]
    protected Task() : base() { }
    protected Task(
        string title,
        string? taskDescription = null,
        TaskPriority? priority = TaskPriority.Normal,
        TaskStatus? status = TaskStatus.Pending,
        DateTime? dueTime = null,
        string? cancelReason = null)
    {
        Id = Guid.NewGuid();
        Title = title.Trim();
        TaskDescription = taskDescription?.Trim() ?? "Sin descripción.";
        Priority = priority;
        Status = status;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        DueTime = dueTime;

        // if (CancelReason is not null && string.IsNullOrWhiteSpace(CancelReason))
        //     throw new ArgumentException("EL motivo de cancilacion no puede estar vacio.");
        //TODO: Aquí debería considerar añadir logica para según el estado de la tarea, si no se aporta motivo de cancelación, es null, se asigna un motivo por defecto
        //  
        // CancelReason = value?.Trim() ?? "Sin motivo de cancelación.";

        CancelReason ??= $"Tarea no cancelada. Estado: {Status.ToString()}";
    }

    public void RenameTask(string newTitle)
    {

        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Título vacío");

        if (newTitle.Length > 20)
            throw new ArgumentException("Máx 20 caracteres");

        Title = newTitle.Trim();
    }

    public void UpdateTaskDescription(string newTaskDescription)
    {
        this.TaskDescription = newTaskDescription;
    }

    public void ChangePriority(TaskPriority newTaskPriority)
    {
        this.Priority = newTaskPriority;
    }
    public void UpdateDueTime(DateTime newDueTime)
    {

        if (newDueTime <= DateTime.Now)
            throw new ArgumentException("La fecha introducida para su vencimiento no puede ser anterior a la actual.");
        else if (newDueTime > DateTime.Now.AddYears(2))
            throw new ArgumentException("La fecha de fin de tarea es mayor a 2 años, No es una fecha válida.");
        DueTime = newDueTime;

    }
    public bool CompleteTask()
    {
        if (Status != TaskStatus.Completed && Status != TaskStatus.Cancelled)
        {
            Status = TaskStatus.Completed;
            UpdatedAt = DateTime.Now;
            return true;
        }

        return false;

    }

    public bool ReopenTask()
    {
        if (this.Status != TaskStatus.InProgress)
        {
            this.Status = TaskStatus.InProgress;
            this.UpdatedAt = DateTime.Now;
            return true;
        }
        return false;
    }

    public void CancelTask(string cancelReason)
    {
        if (this.Status != TaskStatus.Completed && this.Status != TaskStatus.Cancelled)
        {
            this.CancelReason = cancelReason ?? "No se aporta motivo.";
            this.Status = TaskStatus.Cancelled;
            this.UpdatedAt = DateTime.Now;
        }
        else
        {
            throw new Exception($"La tarea no se pudo Cancelar porque la tarea estaba {this.Status}");
        }
    }

    public void StartTask()
    {
        if (this.Status == TaskStatus.Pending)
        {
            this.Status = TaskStatus.InProgress;
            this.UpdatedAt = DateTime.Now;
        }
        else
        {
            throw new Exception($"La Tareas no se pudo iniciar porque la tarea está {this.Status}");
        }
    }
    public bool IsOverdue()
    {
        if (this.DueTime is null)
            return false;

        if (this.Status == TaskStatus.Completed || this.Status == TaskStatus.Cancelled)
            return false;

        return DateTime.Now > this.DueTime;
    }

    public int CalculateDays()
    {
        if (this.DueTime is null)
            throw new ArgumentException("No existe fecha de fin establecida.");

        TimeSpan daysDiference = (TimeSpan)(DateTime.Now - this.DueTime);
        return (int)daysDiference.Days;

    }

    public abstract string ResumeTask();

}

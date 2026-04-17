using System;
using System.Data.Common;
using System.Diagnostics.Tracing;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using GestorTareas.Interfaces;



namespace GestorTareas.Models;

//TODO: Revisar
//1. Configura la clase base con atributos de polimorfismo
// [System.Text.Json.Serialization.JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
// [System.Text.Json.Serialization.JsonDerivedType(typeof(SimpleTask),0)]
// [System.Text.Json.Serialization.JsonDerivedType(typeof(RecurringTask), 1)]
// [System.Text.Json.Serialization.JsonDerivedType(typeof(CompositeTask), 2)]
// [System.Text.Json.Serialization.JsonDerivedType(typeof(SubTask), 3)]
// [System.Text.Json.Serialization.JsonDerivedType(typeof(LinkedTask), 4)]


//2. Configura la clase base con atributos de polimorfismo

[System.Text.Json.Serialization.JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[System.Text.Json.Serialization.JsonDerivedType(typeof(SimpleTask), "SimpleTask")]
[System.Text.Json.Serialization.JsonDerivedType(typeof(RecurringTask), "RecurringTask")]
[System.Text.Json.Serialization.JsonDerivedType(typeof(CompositeTask), "CompositeTask")]
[System.Text.Json.Serialization.JsonDerivedType(typeof(SubTask), "SubTask")]
[System.Text.Json.Serialization.JsonDerivedType(typeof(LinkedTask), "LinkedTask")]

public abstract class Task : IIdentificable, ITaskDisplayable
{
    public enum TaskPriority
    {
        Low,
        Normal,
        High,
        Critical
    }

    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }

    public Guid Id { get; set; }
    public string Title
    {
        get;
        set
        {
            Console.WriteLine(value);
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("El título no puede estar vacío");
            if (value.Length > 20)
                throw new Exception("El título no puede contener más de 20 caracteres");

            field = value.Trim();
        }
    }

    public string? Description { get; set; }

    public TaskPriority Priority { get; set; }

    private TaskStatus _status;
    public TaskStatus Status { get => this._status; set => this._status = value; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DueTime { get; set; }
    public string? CancelReason{get; set;} = null;
    
    // Constructor vacio para trabajar la serialización
    // con polimorfismo
    [JsonConstructor]
    protected Task() : base() { } 
    protected Task(
        string title,
        string? description = null,
        TaskPriority? priority = TaskPriority.Normal,
        TaskStatus? status = TaskStatus.Pending,
        DateTime? dueTime = null)
    {
        // if (string.IsNullOrWhiteSpace(title))
        //     throw new ArgumentException("El título es obligatorio");
        // if (fechaLimite.Date < DateTime.Today)
        //     throw new ArgumentException(
        //     "La fecha límite no puede ser anterior a hoy");


        this.Id = Guid.NewGuid();
        this.Title = title;
        this.Description=description?.Trim()??"Undefined";
        this.Priority = priority ?? TaskPriority.Normal;
        this.Status = status ?? TaskStatus.Pending;
        this.CreatedAt = DateTime.Now;
        this.UpdatedAt = DateTime.Now;
        this.DueTime = dueTime;
        this.CancelReason ??= $"Tarea no cancelada. Estdo: {this.Status.ToString()}";
    }

    public void RenameTask(string newTitle)
    {
        this.Title = newTitle;
    }

    public void UpdateDescription(string newDescription)
    {
        this.Description = newDescription;
    }

    public void ChangePriority(TaskPriority newTaskPriority)
    {
        this.Priority = newTaskPriority;
    }
    public void SetDueTime(DateTime newDueTime)
    {
        this.DueTime = newDueTime;
    }
    public bool CompleteTask()
    {
        if (this.Status != TaskStatus.Completed || this.Status != TaskStatus.Cancelled)
        {
            this.Status = TaskStatus.Completed;
            this.UpdatedAt=DateTime.Now;
            return true;
        }

        return false;

    }

    public bool ReopenTask()
    {
        if(this.Status != TaskStatus.InProgress)
        {
            this.Status = TaskStatus.InProgress;
            this.UpdatedAt=DateTime.Now;
            return true;
        }
            return false;
    }

    public void CancelTask(string cancelReason)
    {
        if(this.Status != TaskStatus.Completed  || this.Status != TaskStatus.Cancelled){
            this.CancelReason = cancelReason??"No se aporta motivo.";
            this.Status = TaskStatus.Cancelled;
            this.UpdatedAt=DateTime.Now;
        }
        else
        {
            throw new Exception($"La tarea no se pudo Cancelar porque la tarea estaba {this.Status}");
        }
    }

    public void StartTask()
    {
        if(this.Status == TaskStatus.Pending){
            this.Status = TaskStatus.InProgress;
            this.UpdatedAt=DateTime.Now;
        }
        else
        {
            throw new Exception($"La Tareas no se pudo iniciar porque la tarea está {this.Status}");
        }
    }
    public bool IsOverdue()
    {
        if (this.Status != TaskStatus.Completed || this.Status != TaskStatus.Cancelled)
            return false;
        
        return DateTime.Now>this.DueTime;
    }

    public int CalculateDays()
    {
        if(this.DueTime is null)
            throw new Exception("No existe fecha de fin establecida.");
        
        TimeSpan daysDiference = (TimeSpan)(DateTime.Now - this.DueTime);
        return (int)daysDiference.Days;
        
    }

    public abstract string ResumeTask();

}

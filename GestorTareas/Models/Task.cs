using System;
using System.Diagnostics.Tracing;
using System.Net;

namespace GestorTareas.Models;

public abstract class Task
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
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("El título no puede estar vacío");
            // if (Convert.ToString(value))
            //     throw new Exception("El título no tiene un formato válido");
            if (value.Length > 20)
                throw new Exception("El título no puede contener más de 20 caracteres");

            field = value.Trim();
        }
    }

    public string Description { get; set; }

    public TaskPriority Priority { get; set; }

    private TaskStatus _status;
    public TaskStatus Status { get => this._status; set => this._status = value; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DueTime { get; set; }

    private string? _cancelReason;
    public string? CancelReason
    {
        get; set => field = value;
    }

    // public string? TaskCategory {get; set;}
    // public Guid OwnerId { get; set; }

    //Las llevo a otra CLASE para Composicion
    // public DateTime? DueTime { get; set; }

    // public DateTime? CompletedAt { get; set; }

    // public List<string> Tags{get;set;}
    // public Tag tag { get; set; }

    protected Task(string title, string description, TaskPriority priority = TaskPriority.Normal, TaskStatus status = TaskStatus.Pending)
    {
        // if (string.IsNullOrWhiteSpace(title))
        //     throw new ArgumentException("El título es obligatorio");
        // if (fechaLimite.Date < DateTime.Today)
        //     throw new ArgumentException(
        //     "La fecha límite no puede ser anterior a hoy");


        this.Id = Guid.NewGuid();
        this.Title = title;
        this.Description = description;
        this.Priority = priority;
        this.Status = status;
        this.CreatedAt = DateTime.Now;
        this.UpdatedAt = null;
        this.DueTime = null;
        this.CancelReason = null;
    }

    public void RenameTask(string newTitle)
    {

    }

    public void UpdateDescription(string newDescription)
    {

    }

    public void ChangePriority() { }
    public void AssignOwner(Guid ownerId) { }

    public void SetDueTime(DateTime dueTime) { }
    public void AddTag(string tag) { }
    public void RemoveTag(string tag) { }
    public bool CompleteTask()
    {
        return true;
    }

    public bool ReopenTask()
    {
        return true;
    }

    public void CancelTask(string reason)
    {
        this.CancelReason = reason;
        this.Status = TaskStatus.Cancelled;
    }

}

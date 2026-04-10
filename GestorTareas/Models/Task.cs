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

     private string _title;
    public Guid Id { get; }

    public string _Title { get; set;
        // {
        //     if(string.IsNullOrEmpty(_title)) 
        //         Console.WriteLine("No puede ser vacio");
        // }
        }

    public string Description { get; set; }

    public TaskPriority Priority { get; set; }

    public TaskStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

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


            this.Id = new Guid();
            this._Title = title;
            this.Description = description;
            this.Priority = priority;
            this.Status = status;
            this.CreatedAt = DateTime.Now;
    }

    public bool CompleteTask()
    {
        return true;
    }

    public bool ReopenTask()
    {
        return true;
    }

}

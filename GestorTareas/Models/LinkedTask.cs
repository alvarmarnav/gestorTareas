using System;
using System.Text.Json.Serialization;

namespace GestorTareas.Models;

public class LinkedTask : Task
{
    //TODO: pendiente linkedTask logica de dependencias.
    public int? LinkedTaskOrder { get; set; }

// ESTO ES LO QUE FALTA:
    [JsonConstructor]
    public LinkedTask() : base() { } 
    public LinkedTask(
        string title,
        string? description,
        TaskPriority? taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        int? linkedTaskOrder = null) : base(
            title,
            description,
            taskPriority,
            taskStatus,
            dueTime)
    {
        this.LinkedTaskOrder = linkedTaskOrder;
    }

    public void UpdateLinkedTaskOrder(int newOrder) { }

    public void CanStartLinkedTask(Guid linkedTaskId) { }

    public void CompleteLinkedTask(Guid linkedTaskId) { }

    public override string ResumeTask()
    {
        throw new NotImplementedException();    
    }

}

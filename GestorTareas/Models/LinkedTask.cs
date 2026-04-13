using System;

namespace GestorTareas.Models;

public class LinkedTask : CompositeTask
{
    public int? LinkedTaskOrder { get; set; }

    public LinkedTask(string title, string description, TaskPriority taskPriority, TaskStatus taskStatuts, int? linkedTaskOrder = null) : base(title, description, TaskPriority.Normal, TaskStatus.Pending)
    {
        this.LinkedTaskOrder = linkedTaskOrder;
    }

    public void UpdateLinkedTaskOrder(int newOrder) { }

    public void CanStartLinkedTask(Guid linkedTaskId) { }

    public void CompleteLinkedTask(Guid linkedTaskId) { }

}

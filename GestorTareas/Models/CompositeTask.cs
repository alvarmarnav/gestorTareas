using System;

namespace GestorTareas.Models;

public class CompositeTask : Task
{
    public List<Task>? TaskList{get;set;}

    public double Progress{get;set;}

    public int Order{get;set;}

    public CompositeTask(string title, string description, TaskPriority taskPriority, TaskStatus taskStatuts, List<Task> taskList, double progress, int order) : base(title, description, TaskPriority.Normal, TaskStatus.Pending)
    {
        this.TaskList = taskList;
        this.Progress = progress;
        this.Order = order;
    }

    public void AddSubTask(Task task)
    {
        
    }

    public void RemoveSubTask(Guid taskId)
    {
        
    }
}

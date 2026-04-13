using System;

namespace GestorTareas.Models;

public class CompositeTask : Task
{
    public List<SubTask>? SubTaskList { get; set; }

    // public int? LinkedTaskOrder{get;set;}

    // public double Progress { get; set; }
    public CompositeTask(string title, string description, TaskPriority taskPriority, TaskStatus taskStatuts) : base(title, description, TaskPriority.Normal, TaskStatus.Pending)
    {
        this.SubTaskList = new List<SubTask>(10);

    }

    public void AddSubTask(SubTask subTask, int order)
    {

    }

    public void ReorderSubTask(Guid subTaskId, int newOrder)
    {

    }

    //CREO QUE LA ESTO DEBERIA HACERLO LA CLASE SUBTASK, YA QUE ES LA QUE TIENE EL ESTADO DE CADA SUBTAREA
    // public void CompleteSubTask(Guid taskId)
    // {

    // }

    public void CalculateProgress() { }
}

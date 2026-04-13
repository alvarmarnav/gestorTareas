using System;
using System.Linq;
namespace GestorTareas.Models;

public class CompositeTask : Task
{
    private List<SubTask> _SubTaskList { get; set; }

    // public int? LinkedTaskOrder{get;set;}

    // public double Progress { get; set; }
    public CompositeTask(
        string title,
        string description,
        TaskPriority taskPriority,
        TaskStatus taskStatuts,
        DateTime dueTime) : base(
            title,
            description,
            TaskPriority.Normal,
            TaskStatus.Pending,
            dueTime)
    {
        this._SubTaskList = new List<SubTask>(10);
    }

    public void AddSubTask(SubTask subTask, int order)
    {
        _SubTaskList.Insert(--order,subTask);
    }

    public void ReorderSubTask(Guid subTaskId, int newOrder)
    {

    }

    //CREO QUE LA ESTO DEBERIA HACERLO LA CLASE SUBTASK, YA QUE ES LA QUE TIENE EL ESTADO DE CADA SUBTAREA
    // public void CompleteSubTask(Guid taskId)
    // {

    // }
    public int CountSubTasks()=>_SubTaskList.Count();

    public decimal CalculateProgress()
    {
        int completedSubTask = _SubTaskList.Count(t=>t.Status==TaskStatus.Completed);

       return (completedSubTask/CountSubTasks())*100;
    }

    public override string ResumeTask()
    {
                return $"Tarea con Subtareas\nTitulo: {this.Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}";
    }
}

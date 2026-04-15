using System;
using System.Linq;
namespace GestorTareas.Models;

public class CompositeTask : Task
{
    // //ReadOnly, asegura que una vez creada la instancia solo se va a crear una vez
    //Lo ccambio de private a protected para poder leer desde la clase hija
    protected readonly List<SubTask> _subTaskList = new List<SubTask>();
    // private  List<SubTask> _SubTaskList { get; set; }

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
        //Esto solo tiene sentido si la creamos con el setter
        // this._SubTaskList = new List<SubTask>();
    }

    public void AddSubTask(SubTask subTask, int order)
    {
        _subTaskList.Insert(--order,subTask);
    }

    public void ReorderSubTask(Guid subTaskId, int newOrder)
    {
        //TODO: Implementar metodo para reorganizar subtareas.
        //Metodos Remove e Insert


        var subTaskSelectedList = this._subTaskList
        .Where(sub => sub.Id.Equals(subTaskId));
      
        int subTaskCounted = CountSubTasks();

        if(newOrder>subTaskCounted || newOrder<=0)
            throw new ArgumentException("Posición nó válida");
        
        _subTaskList.Remove(subTaskSelectedList.ElementAt(0));
        _subTaskList.Insert(--newOrder,subTaskSelectedList.ElementAt(0));

    }

    public int CountSubTasks()=>_subTaskList.Count();

    public decimal CalculateProgress()
    {
        int completedSubTask = _subTaskList.Count(t=>t.Status==TaskStatus.Completed);

       return (completedSubTask/CountSubTasks())*100;
    }

    public SubTask CreateSubTask(
        string subTaskTitle,
        string subTaskDescription,
        TaskPriority subTaskPriority,
        TaskStatus subTaskStatus,
        DateTime dueTime,
        int? subTaskOrder)
    {
        return new SubTask(
            subTaskTitle,
            subTaskDescription,
            subTaskPriority,
            subTaskStatus,
            dueTime,
            subTaskOrder);
    }
    public override string ResumeTask()
    {
                return $"Tarea con Subtareas\nTitulo: {this.Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}\nFecha Limite: {this.DueTime}\nNumero Subtareas: {this._subTaskList.Count()}";
    }
}

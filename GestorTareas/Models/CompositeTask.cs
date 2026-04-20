using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
namespace GestorTareas.Models;

public class CompositeTask : Task
{
    // //ReadOnly, asegura que una vez creada la instancia solo se va a crear una vez
    //Lo ccambio de private a protected para poder leer desde la clase hija
    protected readonly List<SubTask> _subTaskList = new List<SubTask>();

    protected readonly List<LinkedTask> _linkedTaskList = new List<LinkedTask>();

    [JsonConstructor]
    public CompositeTask() : base() { }

    public CompositeTask(
        string title,
        string? description = null,
        TaskPriority? taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending,
        DateTime? dueTime = null) : base(
            title,
            description,
            taskPriority,
            taskStatus,
            dueTime)
    {

    }

    public void AddSubTask(SubTask subTask, int order)
    {
        _subTaskList.Insert(--order, subTask);
    }

    public void ReorderSubTask(Guid subTaskId, int newOrder)
    {
        //TODO: Implementar metodo para reorganizar subtareas.
        //Metodos Remove e Insert

        try
        {
            SubTask subTaskSelected = this._subTaskList.First(sub => sub.Id == subTaskId);

            if (newOrder > CountSubTasks() || newOrder <= 0)
                throw new ArgumentException("Posición nó válida");


            SubTask tempSubTask = subTaskSelected;

            _subTaskList.Remove(subTaskSelected);

            _subTaskList.Insert(--newOrder, tempSubTask);

        }
        catch (Exception)
        {

        }

    }

    public int CountSubTasks() => _subTaskList.Count();

    public decimal CalculateProgress()
    {
        int completedSubTask = _subTaskList.Count(t => t.Status == TaskStatus.Completed);

        return (completedSubTask / CountSubTasks()) * 100;
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


    public void AddLinkedTask(
        string linkedTaskTitle,
        string linkedTaskDescription,
        TaskPriority linkedTaskPriority,
        TaskStatus linkedTaskStatus,
        DateTime dueTime,
        int? order)
    {

        if (order < 0)
            throw new ArgumentException("La posición no es válida.");
        //devuelve true si hay algo en la list
        else if (!_linkedTaskList.Any())
        {
            order = 0;
        }
        else if (order is null || order > _linkedTaskList.Count())
        {
            order = _linkedTaskList.Count();
        }
        else
        {
            foreach (var i in _linkedTaskList
            .Where(i => i.Order > order))
            {
                if (i.Status == TaskStatus.Completed)
                    throw new ArgumentException("No se puede insertar una tarea pendiente nueva previa a tareas ya completadas.");
            }

        }
        LinkedTask linkedTask = CreateLinkedTask(
            linkedTaskTitle,
            linkedTaskDescription,
            linkedTaskPriority,
            linkedTaskStatus,
            dueTime,
            (int)order);

        _linkedTaskList.Insert(order.Value, linkedTask);

        foreach (var item in _linkedTaskList.Where(item => item.Order >= order))
        {
            ++item.Order;
        }

    }
    public LinkedTask CreateLinkedTask(
        string linkedTaskTitle,
        string linkedTaskDescription,
        TaskPriority linkedTaskPriority,
        TaskStatus linkedTaskStatus,
        DateTime dueTime,
        int order
        )
    {
        return new LinkedTask(
         linkedTaskTitle,
         linkedTaskDescription,
         linkedTaskPriority,
         linkedTaskStatus,
         dueTime,
         order
          );
    }

    // public bool CanStartLinkedTask(List<LinkedTask> linkedTasks, LinkedTask lTask)


    public override string ResumeTask()
    {
        return $"Tarea con Subtareas\nTitulo: {this.Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}\nFecha Limite: {this.DueTime}\nNumero Subtareas: {this._subTaskList.Count()}";
    }
}

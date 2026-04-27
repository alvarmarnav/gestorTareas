using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using GestorTareas.Enums;
using TaskStatus = GestorTareas.Enums.TaskStatus;
namespace GestorTareas.Models;

public class CompositeTask : Task
{

    public CompositeTaskType CompositeTaskType
    {
        get; set
        {
            if (!Enum.IsDefined(typeof(CompositeTaskType), value))
                throw new ArgumentException("No es un valor válido.");
            field = value;
        }
    }

    // //ReadOnly, asegura que una vez creada la instancia solo se va a crear una vez
    //Lo ccambio de private a protected para poder leer desde la clase hija
    public List<SubTask> SubTaskList { get; set; } = new List<SubTask>();

    private const int _MAX_ITEMS = 30;
    public List<LinkedTask> LinkedTaskList { get; set; } = new List<LinkedTask>();

    [JsonConstructor]
    public CompositeTask() : base() { }

    public CompositeTask(
        string title,
        CompositeTaskType compositeTaskType,
        string? description = null,
        TaskPriority? taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending,
        DateTime? dueTime = null
        ) : base(
            title,
            description,
            taskPriority,
            taskStatus,
            dueTime)
    {
    }

    public void AddSubTask(
        string subTaskTitle,
        CompositeTaskType compositeTaskType,
        string subTaskDescription,
        TaskPriority subTaskPriority,
        TaskStatus subTaskStatus,
        DateTime dueTime)
    {

        //Validar no exceder n MAX SubTask permitidas
        if (SubTaskList.Count() == _MAX_ITEMS)
            throw new ArgumentOutOfRangeException("Se ha intentado añadir un número de tareas superior al admitido.");

        SubTask subTask = CreateSubTask(
            subTaskTitle,
            compositeTaskType,
            subTaskDescription,
            subTaskPriority,
            subTaskStatus,
            dueTime);

        SubTaskList.Add(subTask);
    }

    // public void ReorderSubTask(Guid subTaskId, int newOrder)
    // {
    //     //TODO: Implementar metodo para reorganizar subtareas.
    //     //Metodos Remove e Insert

    //     try
    //     {
    //         SubTask subTaskSelected = this.subTaskList.First(sub => sub.Id == subTaskId);

    //         if (newOrder > CountSubTasks() || newOrder <= 0)
    //             throw new ArgumentException("Posición nó válida");


    //         SubTask tempSubTask = subTaskSelected;

    //         subTaskList.Remove(subTaskSelected);

    //         subTaskList.Insert(--newOrder, tempSubTask);

    //     }
    //     catch (Exception)
    //     {

    //     }

    // }

    public SubTask CreateSubTask(
        string subTaskTitle,
        CompositeTaskType compositeTaskType,
        string subTaskDescription,
        TaskPriority subTaskPriority,
        TaskStatus subTaskStatus,
        DateTime dueTime
        )
    {
        return new SubTask(
            subTaskTitle,
            compositeTaskType,
            subTaskDescription,
            subTaskPriority,
            subTaskStatus,
            dueTime
            );
    }

    public void AddLinkedTask(
        string linkedTaskTitle,
        CompositeTaskType compositeTaskType,
        string linkedTaskDescription,
        TaskPriority linkedTaskPriority,
        TaskStatus linkedTaskStatus,
        DateTime dueTime,
        int? order)
    {
        if (this.LinkedTaskList.Count == _MAX_ITEMS)
            throw new ArgumentOutOfRangeException("Se ha intentado añadir un número de tareas superior al admitido.");

        if (order < 0)
            throw new ArgumentException("La posición no es válida.");
        //devuelve true si hay algo en la list
        else if (!LinkedTaskList.Any())
        {
            order = 0;
        }
        else if (order is null || order > LinkedTaskList.Count())
        {
            order = LinkedTaskList.Count();
        }
        else
        {
            foreach (var i in LinkedTaskList
            .Where(i => i.Order > order))
            {
                if (i.Status == TaskStatus.Completed)
                    throw new ArgumentException("No se puede insertar una tarea pendiente nueva previa a tareas ya completadas.");
            }

        }
        LinkedTask linkedTask = CreateLinkedTask(
            linkedTaskTitle,
            compositeTaskType,
            linkedTaskDescription,
            linkedTaskPriority,
            linkedTaskStatus,
            dueTime,
            (int)order);

        LinkedTaskList.Insert(order.Value, linkedTask);

        foreach (var item in LinkedTaskList.Where(item => item.Order >= order))
        {
            ++item.Order;
        }

    }
    public LinkedTask CreateLinkedTask(
        string linkedTaskTitle,
        CompositeTaskType compositeTaskType,
        string linkedTaskDescription,
        TaskPriority linkedTaskPriority,
        TaskStatus linkedTaskStatus,
        DateTime dueTime,
        int order
        )
    {
        return new LinkedTask(
            linkedTaskTitle,
            compositeTaskType,
            linkedTaskDescription,
            linkedTaskPriority,
            linkedTaskStatus,
            dueTime,
            order
          );
    }

    // public bool CanStartLinkedTask(List<LinkedTask> linkedTasks, LinkedTask lTask)


    // public int CountSubTasks() => SubTaskList.Count;

    public int CountTasks()
    {
        if (this.CompositeTaskType == CompositeTaskType.SubTask)
        {
            return SubTaskList.Count;
        }
        return LinkedTaskList.Count;
    }

    public decimal CalculateProgress()
    {
        int completedTasks;
        // int numberTasks;

        if (this.CompositeTaskType == CompositeTaskType.SubTask)
        {
            completedTasks = SubTaskList.Count(t => t.Status == TaskStatus.Completed);

        }
        else
        {
            completedTasks = LinkedTaskList.Count(t => t.Status == TaskStatus.Completed);
        }

        return (decimal)completedTasks / CountTasks() * 100;
    }


    //TODO: debo añadir funcionalidad según CompositeTaskTYPE
    // public decimal CalculateProgress()
    // {
    //     int completedSubTask = SubTaskList.Count(t => t.Status == TaskStatus.Completed);

    //     return (completedSubTask / CountSubTasks()) * 100;
    // }

    public void ChangeStatus(TaskStatus newStatus)
    {
        if (!Enum.IsDefined(typeof(TaskStatus), newStatus))
            throw new ArgumentException("El estado no es válido");

        if (newStatus == TaskStatus.Completed)
        {
            if (this.CalculateProgress() != 100)
                throw new ArgumentException("No se puede completar la Tarea padre sin tener todas las subtareas completadas.");
        }
        this.Status = newStatus;
    }

    public override string ResumeTask()
    {
        return $"Tarea con Subtareas\nTitulo: {this.Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}\nFecha Limite: {this.DueTime}\nNumero Subtareas: {this.SubTaskList.Count()}";
    }
}

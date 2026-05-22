using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using TaskStatus = GestorTareas.Enums.TaskStatus;
namespace GestorTareas.Models;

public class CompositeTask : Task
{
    public List<SubTask> SubTaskList { get; set; } = new List<SubTask>();
    private const int _MAX_ITEMS = 30;
    [JsonConstructor]
    public CompositeTask() : base() { }
    public CompositeTask(
        string title,
        int userId,
        string? taskDescription = null,
        TaskPriority? taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        string? cancelReason = null
        ) : base(
            title,
            userId,
            taskDescription,
            taskPriority,
            taskStatus,
            dueTime,
            cancelReason)
    {
        SubTaskList = new List<SubTask>(_MAX_ITEMS);
    }

    public void AddSubTask(
        string subTaskTitle,
        int userId,
        CompositeTaskType compositeTaskType,
        string subTaskDescription,
        TaskPriority subTaskPriority,
        TaskStatus subTaskStatus,
        DateTime? dueTime)
    {

        //Validar no exceder n MAX SubTask permitidas
        if (SubTaskList.Count >= _MAX_ITEMS)
            throw new ArgumentOutOfRangeException("Se ha intentado añadir un número de tareas superior al admitido.");

        SubTask subTask = new SubTask(
            subTaskTitle,
            userId,
            this.Id,
            subTaskDescription,
            subTaskPriority,
            subTaskStatus,
            dueTime);

        SubTaskList.Add(subTask);
    }
    public decimal CalculateProgress()
    {
        int totalTasks = SubTaskList.Count;
        if(totalTasks==0)
        return 0;

        int completedTasks = SubTaskList.Count(t => t.Status == TaskStatus.Completed);
        return (decimal)completedTasks /totalTasks * 100;
    }
    public void ChangeStatus(TaskStatus newStatus)
    {
        if (!Enum.IsDefined(typeof(TaskStatus), newStatus))
            throw new ArgumentException("El estado no es válido");

        if (newStatus == TaskStatus.Completed)
        {
            if (this.CalculateProgress() != 100)
                throw new ArgumentException("No se puede completar la Tarea padre sin tener todas las subtareas completadas.");
        }
        base.Status = newStatus;
    }

    public override string ResumeTask() => $"Tarea con Subtareas\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {Priority}\nEstado: {Status}\nFecha Limite: {DueTime}\nNumero Subtareas: {SubTaskList.Count}";

}

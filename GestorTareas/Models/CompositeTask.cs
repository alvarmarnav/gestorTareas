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
    public const int MaxSubTasks = 30;
    [JsonConstructor]
    public CompositeTask() : base() { }
    public CompositeTask(
        string title,
        int userId,
        string? taskDescription = null,
        TaskType taskType = TaskType.CompositeTask,
        TaskPriority taskPriority = TaskPriority.Normal,
        TaskStatus taskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        string? cancelReason = null
        ) : base(
            title,
            userId,
            taskDescription,
            taskType,
            taskPriority,
            taskStatus,
            dueTime,
            cancelReason)
    {
        SubTaskList = new List<SubTask>(MaxSubTasks);
    }

    public void AddSubTask(
        string subTaskTitle,
        int userId,
        CompositeTaskType compositeTaskType,
        string subTaskDescription,
        TaskType subTaskType,
        TaskPriority subTaskPriority,
        TaskStatus subTaskStatus,
        DateTime? dueTime)
    {

        //Validar no exceder n MAX SubTask permitidas
        if (SubTaskList.Count >= MaxSubTasks)
            throw new ArgumentOutOfRangeException("Se ha intentado añadir un número de tareas superior al admitido.");

        if (userId != UserId)
            throw new InvalidOperationException("La subtarea tiene que pertenecer al mismo usuario que la tarea padre.");

        SubTask subTask = new SubTask(
            subTaskTitle,
            userId,
            this.Id,
            subTaskDescription,
            subTaskType,
            subTaskPriority,
            subTaskStatus,
            dueTime);

        SubTaskList.Add(subTask);
        AddUpdatedDate();
    }
    public decimal CalculateProgress()
    {
        int totalTasks = SubTaskList.Count;
        if (totalTasks == 0)
            return 0;

        var completedTasks = SubTaskList.Count(t => t.Status == TaskStatus.Completed);
        return Math.Round((decimal)completedTasks / totalTasks * 100,2);
    }
    public override string ResumeTask() => $"Tarea con Subtareas\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {Priority}\nEstado: {Status}\nFecha Limite: {DueTime}\nNumero Subtareas: {SubTaskList.Count}";

    public bool CanBeCompleted()
        => SubTaskList.Count == 0 || SubTaskList.All(t => t.Status == TaskStatus.Completed);

}

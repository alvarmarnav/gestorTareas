using System;
using System.Text.Json.Serialization;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using GestorTareas.Interfaces;
using TaskStatus = GestorTareas.Enums.TaskStatus;
using TaskPriority = GestorTareas.Enums.TaskPriority;
namespace GestorTareas.Models;

public class SubTask : Task
{
    public int ParentCompositeTaskId { get; set; }
    public CompositeTask ParentCompositeTask { get; set; }

    [JsonConstructor]
    public SubTask() : base() { }
    public SubTask(
        string subTaskTitle,
        int userId,
        int parentCompositeTaskId,
        string? subTaskDescription = null,
        TaskType subTaskType=TaskType.SubTask,
        TaskPriority subTaskPriority = TaskPriority.Normal,
        TaskStatus? subTaskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        string? cancelReason = null
        ) : base(
            subTaskTitle,
            userId,
            subTaskDescription,
            subTaskType,
            subTaskPriority,
            subTaskStatus,
            dueTime,
            cancelReason)
    {
        ParentCompositeTaskId = parentCompositeTaskId;
        // ParentCompositeTask = 
    }
    public override string ResumeTask() => $"SubTarea Id: {Id}\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {Priority}\nEstado: {Status}";

}

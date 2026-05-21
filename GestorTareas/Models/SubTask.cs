using System;
using System.Text.Json.Serialization;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using GestorTareas.Interfaces;
using TaskStatus = GestorTareas.Enums.TaskStatus;
using TaskPriority = GestorTareas.Enums.TaskPriority;
namespace GestorTareas.Models;

public class SubTask : CompositeTask
{
    public int CompositeTaskId { get; set; }
    public CompositeTask CompositeTaskFather { get; set; }

    [JsonConstructor]
    public SubTask() : base() { }
    public SubTask(
        string subTaskTitle,
        int userId,
        int compositeTaskId,
        string? subTaskDescription = null,
        TaskPriority? subTaskPriority = TaskPriority.Normal,
        TaskStatus? subTaskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        string? cancelReason = null
        ) : base(
            subTaskTitle,
            userId,
            subTaskDescription,
            subTaskPriority,
            subTaskStatus,
            dueTime,
            cancelReason)
    {
        CompositeTaskId = compositeTaskId;
    }
    public override string ResumeTask() => $"SubTarea Id: {Id}\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {Priority}\nEstado: {Status}";

}

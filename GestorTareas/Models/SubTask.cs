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
    [JsonConstructor]
    public SubTask() : base() { }
    public SubTask(
        string subTaskTitle,
        int userId,
        CompositeTaskType compositeTaskType,
        string? subTaskDescription = null,
        TaskPriority? subTaskPriority = TaskPriority.Normal,
        TaskStatus? subTaskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        string? cancelReason = null
        ) : base(
            subTaskTitle,
            userId,
            compositeTaskType,
            subTaskDescription,
            subTaskPriority,
            subTaskStatus,
            dueTime,
            cancelReason)
    {
        CompositeTaskType = CompositeTaskType.SubTask;
    }

    public void UpdateSubTaskOrder(int newOrder)
    {
    }
    public override string ResumeTask() => $"SubTarea Id: {Id}\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {Priority}\nEstado: {Status}";

}

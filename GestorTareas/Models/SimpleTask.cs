using System;
using System.Data.Common;
using System.Text.Json.Serialization;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Models;


public class SimpleTask : Task
{

    [JsonConstructor]
    public SimpleTask() : base() { } 
    public SimpleTask(
        string title,
        int userId,
        string? taskDescription = null,
        TaskPriority? taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        string? cancelReason = null) : base(
            title,
            userId,
            taskDescription,
            taskPriority,
            taskStatus,
            dueTime,
            cancelReason){}

    public override string ResumeTask() => $"Tarea Simple\nTitulo: {Title}\nDescripción: {this.TaskDescription}\nPrioridad: {this.Priority}\nEstado: {this.Status}";
}

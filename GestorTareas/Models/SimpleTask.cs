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
        string? description = null,
        TaskPriority? taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending,
        DateTime? dueTime = null) : base(
            title,
            description,
            taskPriority,
            taskStatus,
            dueTime){}

    public override string ResumeTask()
    {
        // return $"Tarea Simple\nTitulo: {Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}";
        var taskSummary = new TaskSummaryManager();
        taskSummary.ResumeTask(this);
    }
}

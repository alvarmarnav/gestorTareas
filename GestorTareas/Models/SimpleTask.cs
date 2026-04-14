using System;
using System.Data.Common;

namespace GestorTareas.Models;

public class SimpleTask : Task
{
    public SimpleTask(
        string title,
        string description,
        TaskPriority taskPriority,
        TaskStatus taskStatus,
        DateTime dueTime): base(
            title,
            description,
            TaskPriority.Normal,
            TaskStatus.Pending,
            dueTime)
    {
        
    }

    public override string ResumeTask()
    {
        return $"Tarea Simple\nTitulo: {Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}";

    }
}

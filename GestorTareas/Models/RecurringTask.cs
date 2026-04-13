using System;
using GestorTareas.Interfaces;

namespace GestorTareas.Models;

public class RecurringTask : Task
{
    public int RecurrenceRule { get; set; }

    public RecurringTask(string title, string description, TaskPriority priority, TaskStatus status,DateTime dueTime, int recurrenceRule) : base(title, description, priority, status, dueTime)
    {
        this.RecurrenceRule = recurrenceRule;
    }

    // public void GenerateNewInstance()
    // {
    //     // Lógica para generar una nueva instancia de la tarea recurrente según la regla de recurrencia
    //     new RecurringTask(this.Title, this.Description, this.Priority, this.Status, this.DueTime, this.RecurrenceRule);
    // }

    public bool IsRecurrenceActive() => true;

    public override string ResumeTask()
    {
                return $"Tarea Recurrente\nTitulo: {this.Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}";

    }
}

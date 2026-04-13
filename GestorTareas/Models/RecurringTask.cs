using System;
using GestorTareas.Interfaces;

namespace GestorTareas.Models;

public class RecurringTask : Task, IRecurring
{
    public int RecurrenceRule { get; set; }

    public RecurringTask(string title, string description, TaskPriority priority, TaskStatus status, int recurrenceRule) : base(title, description, priority, status)
    {
        this.RecurrenceRule = recurrenceRule;
    }

    public void GenerateNewInstance()
    {
        // Lógica para generar una nueva instancia de la tarea recurrente según la regla de recurrencia
        new RecurringTask(this.Title, this.Description, this.Priority, this.Status, this.RecurrenceRule);
    }

    public bool IsRecurrenceActive() => true;
}

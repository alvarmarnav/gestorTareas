using System;
using GestorTareas.Interfaces;

namespace GestorTareas.Models;

public class RecurringTask : Task
{
    public int RecurrenceRule { get; set; }

    public RecurringTask(string title,
    string? description,
    TaskPriority priority,
    TaskStatus status,
    DateTime? dueTime,
    int recurrenceRule) : base(title,description,priority,status,dueTime)
    {
        // this.DueTime = dueTime ?? throw new ArgumentNullException(nameof(dueTime),"Una tarea recurrente debe contener fecha de fin.");
        
        // if(recurrenceRule<=0)
        //     throw new ArgumentException("Valor no válido para la recurrencia.");

        // this.RecurrenceRule = recurrenceRule;


    }

    public void GenerateNewInstance()
    {
        // Lógica para generar una nueva instancia de la tarea recurrente según la regla de recurrencia
        
        // DateTime validDueTime = this.DueTime is not null ? (DateTime)this.DueTime : throw new ArgumentNullException(nameof(this.DueTime),"No se puede admitir nulos.");

        new RecurringTask(
            this.Title,
            this.Description,
            this.Priority,
            this.Status,
            ValidateDueTime().AddDays(this.RecurrenceRule),
            this.RecurrenceRule);
    }

    public bool IsRecurrenceActive() => true;

    public override string ResumeTask()
    {
                return $"Tarea Recurrente\nTitulo: {this.Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}";

    }

    public DateTime ValidateDueTime()
    {
                // return this.DueTime = dueTime ?? throw new ArgumentNullException(nameof(dueTime),"Una tarea recurrente debe contener fecha de fin.");
           DateTime validDueTime;
           return validDueTime  = this.DueTime is not null ? (DateTime)this.DueTime : throw new ArgumentNullException(nameof(this.DueTime),"No se puede admitir nulos.");

    }

    public int ValidateRecurrenceRule(int recurrenceRule)
    {
        if(recurrenceRule<=0)
            throw new ArgumentException("Valor no válido para la recurrencia.");

       return recurrenceRule;
    }

    public void UpdateRecurrenceRule(int newRecurrenceRule)
    {
        this.RecurrenceRule = ValidateRecurrenceRule(newRecurrenceRule);
    }

}

using System;
using System.Text.Json.Serialization;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using GestorTareas.Interfaces;
using TaskPriority = GestorTareas.Enums.TaskPriority;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Models;

public class RecurringTask : Task
{
    public int RecurrenceRule
    {
        get;
        set
        {
            if (value <= 0 || value > 365)
                throw new ArgumentException("Valor no válido para la recurrencia.");
            field = value;
        }
    }

    private const int _MAX_INSTANCES = 15;
    public int RecurringTasksCount { get; set; } = 0;

    // ESTO ES LO QUE FALTA:
    [JsonConstructor]
    public RecurringTask() : base() { }
    public RecurringTask(
        string title,
        DateTime dueTime,
        int recurrenceRule,
        string? taskDescription = null,
        TaskPriority? taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending,
        string? cancelReason = null
        ) : base(
            title,
            taskDescription,
            taskPriority,
            taskStatus,
            dueTime,
            cancelReason
            )
    {
        RecurrenceRule = recurrenceRule;
        if(RecurringTasksCount<=0 )
            RecurringTasksCount = 0;
        else{
            RecurringTasksCount = RecurringTasksCount;
        }
    }

    public RecurringTask GenerateNewInstance(
        DateTime dueTime)
    {
        if (RecurringTasksCount >= _MAX_INSTANCES)
            throw new InvalidOperationException("No se admiten más instancias.");

        RecurringTasksCount++;

        DateTime newDueTime = dueTime.AddDays(RecurrenceRule);

        return new RecurringTask(
            title:this.Title,
            dueTime:newDueTime,
            recurrenceRule:this.RecurrenceRule,
            taskDescription:this.TaskDescription,
            taskPriority:this.Priority,
            taskStatus:this.Status,
            cancelReason:CancelReason
            );
    }

    public override string ResumeTask() => $"Tarea Recurrente\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {Priority}\nEstado: {Status}\nFecha Fin: {DueTime}\nRegla Recurrencia: {RecurrenceRule}";

}

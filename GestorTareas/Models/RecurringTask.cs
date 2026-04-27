using System;
using System.Text.Json.Serialization;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using GestorTareas.Interfaces;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Models;

public class RecurringTask : Task
{

    // protected List<RecurringTask> RecurringTaskList { get; set; } = new(60);
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
        int recurringTasksCount = 0,
        string? description = null,
        TaskPriority? taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending,
        string? cancelReason = null
        ) : base(
            title,
            description,
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

        var newDueTime = dueTime.AddDays(RecurrenceRule);

        return new RecurringTask(
            Title,
            newDueTime,
            RecurrenceRule,
            RecurringTasksCount,
            Description,
            Priority,
            Status
            );
    }

    public override string ResumeTask() => $"Tarea Recurrente\nTitulo: {Title}\nDescripción: {Description}\nPrioridad: {Priority}\nEstado: {Status}\nFecha Fin: {DueTime}\nRegla Recurrencia: {RecurrenceRule}";


}

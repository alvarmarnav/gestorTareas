using System;
using System.Text.Json.Serialization;
using GestorTareas.Interfaces;

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
    private int _recurringTasksCount = 0;

    // ESTO ES LO QUE FALTA:
    [JsonConstructor]
    public RecurringTask() : base() { }
    public RecurringTask(
        string title,
        DateTime dueTime,
        int recurrenceRule,
        string? description = null,
        TaskPriority? taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending
        ) : base(
            title,
            description,
            taskPriority,
            taskStatus,
            dueTime
            )
    {
        RecurrenceRule = recurrenceRule;
    }

    public RecurringTask GenerateNewInstance(
        DateTime dueTime)
    {

        if (_recurringTasksCount >= _MAX_INSTANCES)
            throw new InvalidOperationException("No se admiten más instancias.");

        _recurringTasksCount++;

        var newDueTime = dueTime.AddDays(RecurrenceRule);

        return new RecurringTask(
            Title,
            newDueTime,
            RecurrenceRule,
            Description,
            Priority,
            Status
            );
    }

    public override string ResumeTask()
    {
        return $"Tarea Recurrente\nTitulo: {this.Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}\nFecha Fin: {DueTime}\nRegla Recurrencia: {RecurrenceRule}";

    }



}

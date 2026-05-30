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
    } = 7;

    public const int MaxInstances = 100;
    public int RecurringTasksCount
    {
        get; set
        {
            if (value < 0 || value > MaxInstances)
                throw new ArgumentException($"El número de ocurrencias debe estar entre 0 y {MaxInstances}.");

            field = value;

        }
    }

    // ESTO ES LO QUE FALTA:
    [JsonConstructor]
    public RecurringTask() : base() { }
    public RecurringTask(
        string title,
        int userId,
        DateTime? dueTime = null,
        int recurrenceRule = 7,
        int recurringTasksCount = 0,
        string? taskDescription = null,
        TaskType taskType = TaskType.RecurringTask,
        TaskPriority taskPriority = TaskPriority.Normal,
        TaskStatus taskStatus = TaskStatus.Pending,
        string? cancelReason = null
        ) : base(
            title,
            userId,
            taskDescription,
            taskType,
            taskPriority,
            taskStatus,
            dueTime ?? DateTime.UtcNow.AddMicroseconds(50),
            cancelReason
            )
    {
        RecurrenceRule = recurrenceRule;
        RecurringTasksCount = RecurringTasksCount;
    }

    public RecurringTask GenerateNewInstance(
        DateTime dueTime)
    {
        if (RecurringTasksCount >= MaxInstances)
            throw new InvalidOperationException("No se admiten más instancias.");

        var nextDueTime = dueTime.AddDays(RecurrenceRule);
        RecurringTasksCount++;

        return new RecurringTask(
            title: this.Title,
            userId: this.UserId,
            dueTime: nextDueTime,
            recurrenceRule: this.RecurrenceRule,
            recurringTasksCount: this.RecurringTasksCount++,
            taskDescription: this.TaskDescription,
            taskPriority: this.Priority,
            taskStatus: TaskStatus.Pending,
            cancelReason: CancelReason
            );
    }

    public override string ResumeTask() => $"Tarea Recurrente\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {Priority}\nEstado: {Status}\nFecha Fin: {DueTime}\nRegla Recurrencia: {RecurrenceRule}";

}

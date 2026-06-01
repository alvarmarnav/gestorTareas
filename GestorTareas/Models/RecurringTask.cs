using System;
using System.Text.Json.Serialization;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using GestorTareas.Interfaces;
using Priority = GestorTareas.Enums.Priority;
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
    public Guid RecurringSeriesId { get; set; } = Guid.NewGuid();

    [JsonConstructor]
    public RecurringTask() : base() { }
    public RecurringTask(
        string title,
        int userId,
        DateTime? dueTime = null,
        int recurrenceRule = 7,
        int recurringTasksCount = 0,
        Guid? recurringSeriesId = null,
        string? taskDescription = null,
        TaskType taskType = TaskType.RecurringTask,
        Priority taskPriority = Priority.Normal,
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
        RecurringSeriesId = recurringSeriesId ?? Guid.NewGuid();
    }

    public RecurringTask GenerateNewInstance(
        DateTime dueTime)
    {
        if (RecurringTasksCount >= MaxInstances)
            throw new InvalidOperationException("No se admiten más instancias.");

        var nextItem = RecurringTasksCount++;
        var nextDueTime = dueTime.AddDays(RecurrenceRule);
        
        return new RecurringTask(
            title: this.Title,
            userId: this.UserId,
            dueTime: nextDueTime,
            recurrenceRule: RecurrenceRule,
            recurringTasksCount: nextItem,
            recurringSeriesId: RecurringSeriesId, 
            taskDescription: this.TaskDescription,
            taskPriority: this.TaskPriority,
            taskStatus: TaskStatus.Pending,
            cancelReason: CancelReason
            );
    }

    public override string ResumeTask() => $"Tarea Recurrente\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {TaskPriority}\nEstado: {TaskStatus}\nFecha Fin: {DueTime}\nRegla Recurrencia: {RecurrenceRule}";

}

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
        int userId,
        DateTime? dueTime=null,
        int recurrenceRule = 7,
        string? taskDescription = null,
        TaskType taskType =TaskType.RecurringTask,
        TaskPriority taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending,
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

        return new RecurringTask(
            title:this.Title,
            userId:this.UserId,
            dueTime: dueTime.AddDays(RecurrenceRule),
            recurrenceRule:this.RecurrenceRule,
            taskDescription:this.TaskDescription,
            taskPriority:this.Priority,
            taskStatus:TaskStatus.Pending,
            cancelReason:CancelReason
            );
    }

    public override string ResumeTask() => $"Tarea Recurrente\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {Priority}\nEstado: {Status}\nFecha Fin: {DueTime}\nRegla Recurrencia: {RecurrenceRule}";

}

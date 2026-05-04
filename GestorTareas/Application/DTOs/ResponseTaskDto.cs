using System;
using GestorTareas.Enums;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas;

public class ResponseTaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string TaskDescription { get; set; } = string.Empty;
    public TaskPriority TaskPriority { get; set; }
    public TaskStatus TaskStatus { get; set; }
    public DateTime DueTime { get; set; }
    public string? CancelReason { get; set; }

    public ResponseTaskDto() { }
    public ResponseTaskDto(
        Guid id,
        string title,
        string taskDescription,
        TaskPriority taskPriority,
        TaskStatus taskStatus,
        DateTime dueTime,
        String cancelReason
    )
    {
        Id = id;
        Title = title;
        TaskDescription = taskDescription;
        TaskPriority = taskPriority;
        TaskStatus = taskStatus;
        DueTime = dueTime;
        CancelReason = cancelReason;
    }

}

using System;
using GestorTareas.Application.DTOs;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Application.DTOs;

public class ResponseTaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string TaskDescription { get; set; } = string.Empty;
    public TaskType TaskType { get; set; } = TaskType.SimpleTask;
    public Priority TaskPriority { get; set; }
    public TaskStatus TaskStatus { get; set; }
    public DateTime? DueTime { get; set; }
    public string? CancelReason { get; set; }
    public int? ParentCompositeTaskId { get; set; }
    public string? ParentCompositeTaskTitle { get; set; }
}

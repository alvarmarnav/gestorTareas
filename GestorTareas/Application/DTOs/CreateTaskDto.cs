using System;
using System.ComponentModel.DataAnnotations;
using GestorTareas.Enums;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Application.DTOs;

public class CreateTaskDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(200, ErrorMessage = "Longitud máxima de 200 caracteres.")]
    public string Title { get; set; }
    public string? TaskDescription { get; set; } = string.Empty;
    public TaskPriority? Priority { get; set; } = TaskPriority.Normal;
    public TaskStatus? Status { get; set; } = TaskStatus.Pending;
    public DateTime? DueTime { get; set; } = null;
    [MaxLength(200, ErrorMessage = "Longitud máxima de 200 caracteres.")]
    public string? CancelReason { get; set; } = string.Empty;
    public CreateTaskDto(
        string title,
        string? taskDescription,
        TaskPriority taskPriority,
        TaskStatus status,
        DateTime dueTime,
        string cancelReason)
    {
        this.Title = title;
        this.TaskDescription = taskDescription;
        this.Priority = taskPriority;
        this.Status = status;
        this.DueTime = dueTime;
        this.CancelReason = cancelReason;
    }
}

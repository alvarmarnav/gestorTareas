using System;
using System.ComponentModel.DataAnnotations;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Application.DTOs;

public class CreateTaskDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(30, ErrorMessage = "Longitud máxima de 30 caracteres.")]
    public string Title { get; set; } = string.Empty;
    // public int UserId{get;set;}
    [MaxLength(250, ErrorMessage = "Longitud máxima de 250 caracteres.")]
    public string? TaskDescription { get; set; } = string.Empty;
    public TaskPriority? Priority { get; set; } = TaskPriority.Normal;
    public TaskStatus? Status { get; set; } = TaskStatus.Pending;
    public DateTime? DueTime { get; set; } = null;
    [MaxLength(200, ErrorMessage = "Longitud máxima de 200 caracteres.")]
    // public string? CancelReason { get; set; } = string.Empty;
    public int? RecurrenceRule { get; set; } = null;
    public List<TaskCollaborator>? TaskCollaborators { get; set; } = null;
    public List<SubTask>? SubTasks { get; set; } = null;
    public int? CompositeTaskId { get; set; } = null;
    public int? LinkedTaskOrder { get; set; } = null;
    // public int? taskId { get; set; } = null;
    public int? TaskID{get;set;}=null;
    public int? DependsOnTaskId{get;set;}=null;
}

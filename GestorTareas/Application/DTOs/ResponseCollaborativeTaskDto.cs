using System;
using GestorTareas.Enums;
using GestorTareas.Models;

namespace GestorTareas.Application.DTOs;

public class ResponseCollaborativeTaskDto : TaskDTO
{
    public string Title { get; set; } = string.Empty;
    public string? TaskDescription { get; set; } = string.Empty;
    public TaskType TaskType{get;set;}=TaskType.CollaborativeTask;
    public TaskPriority? Priority { get; set; } = TaskPriority.Normal;
    public DateTime? DueTime { get; set; } = null;
    public List<TaskCollaboratorDto> TaskCollaborators { get; set; } = [];
}

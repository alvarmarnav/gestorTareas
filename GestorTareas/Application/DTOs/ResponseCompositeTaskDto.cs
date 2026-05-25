using System;
using GestorTareas.Enums;
using GestorTareas.Models;

namespace GestorTareas.Application.DTOs;

public class ResponseCompositeTaskDto : TaskDTO
{
    public string Title { get; set; } = string.Empty;
    public string? TaskDescription { get; set; } = string.Empty;
    public TaskPriority? Priority { get; set; } = TaskPriority.Normal;
    public DateTime? DueTime { get; set; } = null;
    public List<ResponseSubTaskDto> SubTasksList { get; set; } = [];
}

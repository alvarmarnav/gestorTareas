using System;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas;

public class ResponseTaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string TaskDescription { get; set; } = string.Empty;
    public TaskPriority TaskPriority { get; set; }
    public TaskStatus TaskStatus { get; set; }
    public DateTime DueTime { get; set; }
    public string? CancelReason { get; set; }
    public List<User> UsersList { get; set; } = [];

}

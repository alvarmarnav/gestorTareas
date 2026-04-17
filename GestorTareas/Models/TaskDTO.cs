using System;
using System.ComponentModel.DataAnnotations;
namespace GestorTareas.Models;

public abstract class TaskDTO
{

    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = default!;
    [Required]
    public string Description { get; set; } = default!;
    public Task.TaskPriority Priority { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime DueTime { get; set; }


}

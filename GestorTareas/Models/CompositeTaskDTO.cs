using System;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Models;

public class CompositeTaskDTO
{
public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = default!;
    [Required]
    public string Description { get; set; } = default!;
    public TaskStatus Priority { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime DueTime { get; set; }

    //TODO: pendiente subtasklist
}

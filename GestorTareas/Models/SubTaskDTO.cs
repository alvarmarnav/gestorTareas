using System;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Models;

public class SubTaskDTO
{
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = default!;
    [Required]
    public string Description { get; set; } = default!;
    public TaskStatus Priority { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime DueTime { get; set; }

    public int SubTaskOrder {get;set;}
}

using System;
using System.ComponentModel.DataAnnotations;
using GestorTareas.Enums;

namespace GestorTareas;

public class CreateRecurringTaskDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? TaskDescription { get; set; }

    public TaskPriority? Priority { get; set; } = TaskPriority.Normal;

    [Required(ErrorMessage = "La fecha de vencimiento es obligatoria para una tarea recurrente.")]
    public DateTime DueTime { get; set; }

    [Range(1, 365, ErrorMessage = "La recurrencia debe estar entre 1 y 365 días.")]
    public int RecurrenceRule { get; set; }
}

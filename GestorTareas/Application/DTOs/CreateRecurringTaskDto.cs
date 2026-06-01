using System;
using System.ComponentModel.DataAnnotations;
using GestorTareas.Enums;

namespace GestorTareas.Application.DTOs;

public class CreateRecurringTaskDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(30, ErrorMessage = "El título no puede tener más de 30 caracteres.")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(300, ErrorMessage = "La descripción no puede tener más de 300 caracteres.")]
    public string? TaskDescription { get; set; }

    public Priority TaskPriority { get; set; } = Priority.Normal;
    public TaskType TaskType { get; set; } = TaskType.RecurringTask;
    [Required]
    public DateTime DueTime { get; set; }

    [Range(1, 365, ErrorMessage = "La recurrencia debe estar entre 1 y 365 días.")]
    public int RecurrenceRule { get; set; }
    [Required]
    // [Required(ErrorMessage = "La fecha final de repetición es obligatoria.")]
    public DateTime RepeatUntilDate { get; set; }
    [Range(1, 100,ErrorMessage ="El número de ocurrencias máximo debe estar entre 1 y 100.")]
    public int MaxOcurrences { get; set; } = 20;
}

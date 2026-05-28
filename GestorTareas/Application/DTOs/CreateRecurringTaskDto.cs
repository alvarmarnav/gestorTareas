using System;
using System.ComponentModel.DataAnnotations;
using GestorTareas.Enums;

namespace GestorTareas.Application.DTOs;

public class CreateRecurringTaskDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? TaskDescription { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.Normal;
    public TaskType TaskType{get;set;}=TaskType.RecurringTask;
    public DateTime DueTime { get; set; }

    [Range(1, 365, ErrorMessage = "La recurrencia debe estar entre 1 y 365 días.")]
    public int RecurrenceRule { get; set; }
    public DateTime RepeatUntilDate{get;set;}
    public int MaxOcurrences{get;set;}=20;
}

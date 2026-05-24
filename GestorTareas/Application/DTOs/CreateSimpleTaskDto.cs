using System;
using System.ComponentModel.DataAnnotations;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Application.DTOs;

public class CreateSimpleTaskDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(50, ErrorMessage = "Longitud máxima de 50 caracteres.")]
    public string Title { get; set; } = string.Empty;
    // public int UserId{get;set;}
    [MaxLength(300, ErrorMessage = "Longitud máxima de 300 caracteres.")]
    public string? TaskDescription { get; set; } = string.Empty;
    public TaskPriority? Priority { get; set; } = TaskPriority.Normal;
    public DateTime? DueTime { get; set; } = null;
}

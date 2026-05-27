using System;
using System.ComponentModel.DataAnnotations;
using GestorTareas.Enums;
using GestorTareas.Models;

namespace GestorTareas.Application.DTOs;
public class CreateCompositeTaskDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(50, ErrorMessage = "Longitud máxima de 50 caracteres.")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(300, ErrorMessage = "Longitud máxima de 300 caracteres.")]
    public string? TaskDescription { get; set; }

    public TaskPriority? Priority { get; set; } = TaskPriority.Normal;

    public DateTime? DueTime { get; set; }
    public List<SubTask> SubTaskList {get;set;} = new List<SubTask>(30);
    
}

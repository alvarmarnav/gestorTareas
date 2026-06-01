using System;
using System.ComponentModel.DataAnnotations;
using GestorTareas.Enums;
using GestorTareas.Models;
using Priority = GestorTareas.Enums.Priority;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Application.DTOs;

public class CreateSubTaskDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(30, ErrorMessage = "Longitud máxima de 30 caracteres.")]
    public string Title { get; set; } = string.Empty;
    // public int UserId{get;set;}
    [MaxLength(300, ErrorMessage = "Longitud máxima de 300 caracteres.")]
    public string? TaskDescription { get; set; } = string.Empty;
    public TaskType TaskType{get;set;}=TaskType.SubTask;
    public Priority? TaskPriority { get; set; } = GestorTareas.Enums.Priority.Normal;
    public DateTime? DueTime { get; set; } = null;
    
}

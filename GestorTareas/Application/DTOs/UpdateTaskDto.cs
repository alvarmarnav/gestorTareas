using System;
using System.ComponentModel.DataAnnotations;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Application.DTOs;

public class UpdateTaskDto
{
    [MaxLength(50,ErrorMessage ="Longitud máxima permitida de 50 caracteres.")]
    public string? Title { get; set; } = null;
    [MaxLength(250,ErrorMessage ="Longitud máxima permitida de 250 caracteres.")]
    public string? TaskDescription { get; set; } = null;
    public TaskPriority? Priority { get; set; } = null;
    public TaskStatus? Status { get; set; } = null;
    public DateTime? DueTime { get; set; } = null;
    [Range(0,100,ErrorMessage ="La posición no puede ser negativa.")]
    public int? LinkedTaskOrder { get; set; } = null;
    [Range(1,365,ErrorMessage ="La periodicidad de la tarea no puede ser mayor de un año ni menor de 1 día.")]
    public int? RecurrenceRule { get; set; } = null;
    public User? TaskSupervisor { get; set; } = null;

    public UpdateTaskDto(
        string? title,
        string? taskDescription,
        TaskPriority? priority,
        TaskStatus? status,
        DateTime? dueTime,
        int? linkedTaskOrder,
        int? recurrenceRule,
        User? taskSupervisor
    )
    {
        Title = title;
        TaskDescription = taskDescription;
        Priority = priority;
        Status = status;
        DueTime = dueTime;
        LinkedTaskOrder = linkedTaskOrder;
        RecurrenceRule = recurrenceRule;
        TaskSupervisor = taskSupervisor;
    }

}

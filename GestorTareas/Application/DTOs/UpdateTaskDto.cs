using System;
using System.ComponentModel.DataAnnotations;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Application.DTOs;

public class UpdateTaskDto
{
    [MaxLength(30,ErrorMessage ="Longitud máxima permitida de 30 caracteres.")]
    public string? Title { get; set; } = null;
    [MaxLength(300,ErrorMessage ="Longitud máxima permitida de 300 caracteres.")]
    public string? TaskDescription { get; set; } = null;
    public Priority? TaskPriority { get; set; } = null;
    
    public DateTime? DueTime { get; set; } = null;
    [Range(0,100,ErrorMessage ="La posición no puede ser negativa.")]
    public int? LinkedTaskOrder { get; set; } = null;
    [Range(1,365,ErrorMessage ="La periodicidad de la tarea no puede ser mayor de un año ni menor de 1 día.")]
    public int? RecurrenceRule { get; set; } = null;
    public string? CancelReason{get;set;}=null;
    public UpdateTaskDto(
        string? title,
        string? taskDescription,
        Priority? taskPriority,
        DateTime? dueTime,
        int? linkedTaskOrder,
        int? recurrenceRule,
        string? cancelReason
        )
    {
        Title = title;
        TaskDescription = taskDescription;
        TaskPriority = taskPriority;
        DueTime = dueTime;
        LinkedTaskOrder = linkedTaskOrder;
        RecurrenceRule = recurrenceRule;
        CancelReason = cancelReason;
    }

}

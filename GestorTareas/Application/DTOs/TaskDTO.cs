using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GestorTareas.Enums;
using GestorTareas.Models;

namespace GestorTareas.Application.DTOs;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SimpleTaskDTO), typeDiscriminator: "simple")]
[JsonDerivedType(typeof(ResponseCompositeTaskDto), typeDiscriminator: "composite")]
[JsonDerivedType(typeof(ResponseSubTaskDto), typeDiscriminator: "subtask")]
[JsonDerivedType(typeof(ResponseRecurringTaskDto), typeDiscriminator: "recurring")]
public abstract class TaskDTO
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = default!;
    public int UserId{get;set;}
    public string? TaskDescription { get; set; } = default!;
    public TaskType TaskType{get;set;}=TaskType.SimpleTask;
    public TaskPriority? Priority { get; set; }
    public int? Status { get; set; }
    public DateTime? DueTime { get; set; }
    public string? CancelReason {get;set;}
    public int? LinkedTaskOrder{get;set;}
    public int? RecurrenceRule{get;set;}
}
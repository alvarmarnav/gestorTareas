using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GestorTareas.Enums;
using GestorTareas.Models;

namespace GestorTareas.Application.DTOs;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SimpleTaskDTO), typeDiscriminator: "simple")]
[JsonDerivedType(typeof(CompositeTaskDTO), typeDiscriminator: "composite")]
[JsonDerivedType(typeof(SubTaskDTO), typeDiscriminator: "subtask")]
[JsonDerivedType(typeof(RecurringTaskDTO), typeDiscriminator: "recurring")]
public abstract class TaskDTO
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = default!;
    public int UserId{get;set;}
    public string? TaskDescription { get; set; } = default!;
    public int? Priority { get; set; }
    public int? Status { get; set; }
    public DateTime? DueTime { get; set; }
    public string? CancelReason {get;set;}
    public CompositeTaskType? CompositeTaskType{get;set;}
    public int? LinkedTaskOrder{get;set;}
    public int? RecurrenceRule{get;set;}
    public User? TaskSupervisor{get;set;}

}
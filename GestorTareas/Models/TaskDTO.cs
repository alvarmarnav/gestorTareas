using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace GestorTareas.Models;


[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(SimpleTaskDTO), typeDiscriminator: "simple")]
[JsonDerivedType(typeof(CompositeTaskDTO), typeDiscriminator: "composite")]
[JsonDerivedType(typeof(SubTaskDTO), typeDiscriminator: "subtask")]
[JsonDerivedType(typeof(RecurringTaskDTO), typeDiscriminator: "recurring")]
public abstract class TaskDTO
{
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = default!;
    [Required]
    public string Description { get; set; } = default!;
    public int Priority { get; set; }
    public int Status { get; set; }
    public DateTime DueTime { get; set; }


}

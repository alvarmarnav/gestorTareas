using System;
using GestorTareas.Enums;

namespace GestorTareas.Application.DTOs;

public class TasksRelationsDto
{
public int TaskId { get; set; }
    public TaskType TaskType { get; set; }
    public List<ResponseSubTaskDto> SubTasks { get; set; } = [];
    public List<ResponseRecurringTaskDto> RecurringIterations { get; set; } = [];
    public List<ResponseLinkedTaskDto> LinkedRelations { get; set; } = [];
}

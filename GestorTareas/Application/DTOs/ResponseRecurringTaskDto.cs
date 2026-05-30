using System;
using GestorTareas.Enums;

namespace GestorTareas.Application.DTOs;

public class ResponseRecurringTaskDto : TaskDTO
{
    public int RecurrenceRule { get; set; }
    public int RecurringTasksCount {get; set;}
}

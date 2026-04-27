using System;

namespace GestorTareas.Application.DTOs;

public class RecurringTaskDTO : TaskDTO
{
    public int RecurrenceRule { get; set; }

}

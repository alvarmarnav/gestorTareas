using System;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Models;

public class RecurringTaskDTO : TaskDTO
{
    public int RecurrenceRule { get; set; }

}

using System;
using GestorTareas.Interfaces;

namespace GestorTareas.Models;

public class RecurringTask : Task, IRemindable, IPostponeable, IRecurring
{
    public string RecurrenceRule{get;set;}

    public RecurringTask(string title, string description, TaskPriority priority, TaskStatus status, string recurrenceRule) : base( title, description, priority, status)
    {
        this.RecurrenceRule = recurrenceRule;
    }
}

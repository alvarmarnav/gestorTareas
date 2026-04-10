using System;

namespace GestorTareas.Models;

public class Event:Task
{
    public DateTime EventDate{get;set;}
    public DateTime EventEnd{get;set;}

    public Event(string title, string description, TaskPriority taskPriority, TaskStatus taskStatus, DateTime eventDate, DateTime eventEnd) : base(title,description, TaskPriority.Normal, TaskStatus.Pending)
    {
        this.EventDate = eventDate;
        this.EventEnd = eventEnd;
    }
}

using System;

namespace GestorTareas.Models;

public class SimpleTask : Task
{
    public SimpleTask(string title, string description, TaskPriority taskPriority, TaskStatus taskStatus): base(title,description, TaskPriority.Normal, TaskStatus.Pending)
    {
        
    }
}

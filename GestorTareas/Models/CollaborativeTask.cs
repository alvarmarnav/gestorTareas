using System;

namespace GestorTareas.Models;

public class CollaborativeTask : Task
{
    private List<User>? _TeamMembers { get; set; }

    public CollaborativeTask(
        string title,
        string description,
        TaskPriority taskPriority,
        TaskStatus taskStatus,
        List<User> teamMembers,
        DateTime dueTime) : base(
            title,
            description,
            TaskPriority.Normal,
            TaskStatus.Pending,
            dueTime)
    {
        this._TeamMembers = teamMembers;
    }

    // public CollaborativeTask(string title, string? description = null, TaskPriority? priority = TaskPriority.Normal, TaskStatus? status = TaskStatus.Pending, DateTime? dueTime = null) : base(title, description, priority, status, dueTime)
    // {
    // }

    // public override string ResumeTask()
    // {
    //         return $"Tarea con Subtareas\nTitulo: {this.Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}";
    // }
    public override string ResumeTask()
    {
        throw new NotImplementedException();
    }

    public void AddMember(Guid userId)
    {

    }

    public void RemoveMember(Guid userId) { }

    public List<User> GetMembers()
    {
        // return _TeamMembers;
        throw new NotImplementedException();
    }
}

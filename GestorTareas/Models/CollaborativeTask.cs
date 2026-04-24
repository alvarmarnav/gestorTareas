using System;

namespace GestorTareas.Models;

public class CollaborativeTask : Task
{
    private List<User>? _TeamMembers { get; set; }
    // private User _taskSupervisor;

    public User? TaskSupervisor{get;set;}

    public CollaborativeTask(
        string title,
        string? description,
        TaskPriority? taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending,
        DateTime dueTime,
        User? taskUser
        // List<User> teamMembers,
        ) : base(
            title,
            description,
            taskPriority,
            taskStatus,
            dueTime)
    {
        this.TaskSupervisor = 
        // this._TeamMembers = teamMembers;
    }

    // public override string ResumeTask()
    // {
    //         return $"Tarea con Subtareas\nTitulo: {this.Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}";
    // }
    public override string ResumeTask()
    {
                return $"Tarea Colaborativa\nTitulo: {Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}";
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

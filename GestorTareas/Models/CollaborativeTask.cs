using System;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;

public class CollaborativeTask : GestorTareas.Models.Task
{
    public List<User> TeamMembers { get; set; } = new List<User>(20);
    // private User _taskSupervisor;

    public User TaskSupervisor{get;set;}
//TODO: Incluir en la logica esta CLASE
public CollaborativeTask() : base() { }
    public CollaborativeTask(
        string title,
        User taskSupervisor,
        string? taskDescription = null,
        TaskPriority? taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        string? cancelReason = null
        // List<User> teamMembers,
        ) : base(
            title,
            taskDescription,
            taskPriority,
            taskStatus,
            dueTime,
            cancelReason)
    {
        TeamMembers = new List<User>(20);
        
        this.TaskSupervisor = taskSupervisor;
        TeamMembers.Add(taskSupervisor);
    }

    // public override string ResumeTask()
    // {
    //         return $"Tarea con Subtareas\nTitulo: {this.Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}";
    // }
    public override string ResumeTask() => $"Tarea Colaborativa\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {Priority}\nEstado: {Status}";


    public void AddMember(int userId)
    {

    }

    public void RemoveMember(int userId) { }

    public List<User> GetMembers()
    {
        // return _TeamMembers;
        throw new NotImplementedException();
    }
}

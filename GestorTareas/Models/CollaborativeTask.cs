using System;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Models;

public class CollaborativeTask : GestorTareas.Models.Task
{
    public List<TaskCollaborator> TaskCollaborators { get; set; } = new List<TaskCollaborator>(20);
    
    //TODO: Incluir en la logica esta CLASE
    public CollaborativeTask() : base() { }
    public CollaborativeTask(
        string title,
        int userId,
        string? taskDescription = null,
        TaskType taskType = TaskType.CollaborativeTask,
        TaskPriority taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        string? cancelReason = null
        // List<User> teamMembers,
        ) : base(
            title,
            userId,
            taskDescription,
            taskType,
            taskPriority,
            taskStatus,
            dueTime,
            cancelReason)
    {
        TaskCollaborators = new List<TaskCollaborator>(20);

        TaskCollaborators.Add(new TaskCollaborator
        {
            TaskId = this.Id,
            UserId = this.UserId,
            CollaboratorRole = CollaboratorRole.TaskAdministrator,
        });
    }

    // public override string ResumeTask()
    // {
    //         return $"Tarea con Subtareas\nTitulo: {this.Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}";
    // }
    public override string ResumeTask() => $"Tarea Colaborativa\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {Priority}\nEstado: {Status}";


    public void AddMTaskCollaborator(TaskCollaborator tcollaborator)
    {
        if (TaskCollaborators.Any(tc => tc.UserId == tcollaborator.UserId))
            throw new Exception($"El usuario con ID{tcollaborator.UserId} ya es collaborador.");
            
        this.TaskCollaborators.Add(tcollaborator);
    }

    public void RemoveMember(int userId)
    {
        if (userId > 0)
        {
            var userSelected = this.GetTaskCollaboratorById(userId);
            TaskCollaborators.Remove(userSelected);
        }
    }

    public List<TaskCollaborator> GetTaskCollaborators()
    {
        return TaskCollaborators;
    }
    public TaskCollaborator GetTaskCollaboratorById(int userId)
    {
        var userSelected = TaskCollaborators.FirstOrDefault(i => i.UserId == userId) ?? throw new KeyNotFoundException("No hay usuario con este ID en el Equipo");
        return userSelected;
    }
}

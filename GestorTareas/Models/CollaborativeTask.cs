using System;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Models;

public class CollaborativeTask : GestorTareas.Models.Task
{
    public const int MaxCollaborators = 20;
    public List<TaskCollaborator> TaskCollaborators { get; set; } = new List<TaskCollaborator>(MaxCollaborators);
    public CollaborativeTask() : base() { }
    public CollaborativeTask(
        string title,
        int userId,
        string? taskDescription = null,
        TaskType taskType = TaskType.CollaborativeTask,
        Priority taskPriority = Priority.Normal,
        TaskStatus taskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        string? cancelReason = null
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
        TaskCollaborators =
        [
            new TaskCollaborator
            {
                TaskId = this.Id,
                UserId = this.UserId,
                CollaboratorRole = CollaboratorRole.TaskAdministrator,
            },
        ];
    }
    public override string ResumeTask() => $"Tarea Colaborativa\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {TaskPriority}\nEstado: {TaskStatus}";

    public void AddTaskCollaborator(int userId, CollaboratorRole collaboratorRole)
    {
        if (userId <= 0) throw new ArgumentException("El id del colaborador no es válido.");
        if (this.TaskCollaborators.Count >= MaxCollaborators) throw new InvalidOperationException($"No se pueden añadir más colaboradores a esta tarea, se ha alcanzado el máximo({MaxCollaborators})");

        if (TaskCollaborators.Any(tc => tc.UserId == userId))
            throw new Exception($"El usuario con ID{userId} ya está en el equipo de colaboradores.");

        this.TaskCollaborators.Add(new TaskCollaborator
        {
            TaskId = this.Id,
            Task = this,
            UserId = userId,
            CollaboratorRole = collaboratorRole,
            AddedAt = DateTime.UtcNow,
        });
        AddUpdatedDate();
    }

    public void RemoveCollaborator(int userId)
    {

        var userSelected = TaskCollaborators.FirstOrDefault(tc => tc.UserId == userId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {userId}");
        if (this.UserId == userSelected.UserId) throw new InvalidOperationException($"No se puede eliminar al propietario de la tarea.");
        TaskCollaborators.Remove(userSelected);
        AddUpdatedDate();
    }

    public bool HasCollaborator(int userId)
    => TaskCollaborators.Any(tc => tc.UserId == userId);

    public bool HasTaskAdministrator(int userId)
        => TaskCollaborators.Any(tc => tc.UserId == userId && tc.CollaboratorRole == CollaboratorRole.TaskAdministrator);

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

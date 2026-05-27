using System;
using GestorTareas.Models;
using Task = GestorTareas.Models.Task;
namespace GestorTareas.Interfaces;

public interface ITaskRepository
{
    List<Task> GetAllTasks();
    List<Task> GetAllTasksByUser(int userId);
    Task? GetTaskById(int id);
    Task CreateTask(Task task);
    void DeleteTask(Task task);
    void UpdateTask(Task task);
    (List<Task> tasks, int total) GetTotalPaginated(int page, int ItemsPerPage,int userId, bool? onlyCompletedTask = null,
string? search = null);
    void AddTaskCollaborator(CollaborativeTask collaborativeTask,TaskCollaborator tcollaborator);
    void RemoveTaskCollaborator(CollaborativeTask collaborativeTask,TaskCollaborator tcollaborator);
    bool ExistsCircularRelation(int taskId, int dependsOnTaskId);
    bool ExistsLinkedRelation(int taskId, int dependsOnTaskId);
    LinkedTask AddLinkedRelation(LinkedTask linkedTask);
    void UpdateCompositeTask(int compositeTaskId, SubTask createdTask);
};

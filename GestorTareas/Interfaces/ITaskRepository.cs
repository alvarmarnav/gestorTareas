using System;
using Task = GestorTareas.Models.Task;
namespace GestorTareas.Interfaces;

public interface ITaskRepository
{
    List<Task> GetAllTasks();

    Task? GetTaskById(int id);

    void AddTask(Task task);

    void DeleteTask(Task task);

    void UpdateTask(Task task);
}

using System;
using Task = GestorTareas.Models.Task;
using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Enums;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Application.Services;

public class TaskManagerService
{
    private readonly TaskRepositoryEF _repository;

    public TaskManagerService(TaskRepositoryEF repository) => _repository = repository;

    public List<Task> GetAllTasks() => _repository.GetAllTasks();

    public List<ResponseTaskDto> GetAllTasksDto()
    {
        return _repository.GetAllTasks()
        .Select(t => new ResponseTaskDto
        {
            Id = t.Id,
            Title = t.Title,
            TaskDescription = t.TaskDescription,
            TaskPriority = (TaskPriority)t.Priority,
            TaskStatus = (TaskStatus)t.Status,
            DueTime = (DateTime)t.DueTime,
            CancelReason = t.CancelReason
        }).ToList();
    }
    public Task? GetTaskById(Guid id) => _repository.GetTaskById(id);

    public void AddTask(Task task)
    {
        _repository.AddTask(task);
    }
    public void DeleteTask(Task task)
    {
        _repository.DeleteTask(task);
    }
    public void UpdateTask(Task task)
    {
        _repository.UpdateTask(task);
    }
}
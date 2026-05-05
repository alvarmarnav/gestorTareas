using System;
using Task = GestorTareas.Models.Task;
using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Enums;
using TaskStatus = GestorTareas.Enums.TaskStatus;
using GestorTareas.Models;

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
    public Task? GetTaskById(int id) => _repository.GetTaskById(id);

    public Task AddTask(
        string Title,
        string? TaskDescription,
        TaskPriority? taskPriority,
        TaskStatus? taskStatus,
        DateTime? dueTime)
    {
        var newTask = new SimpleTask
        {
            Title = Title,
            TaskDescription = TaskDescription,
            Priority = taskPriority,
            Status = taskStatus,
            DueTime = dueTime,
            // UserId = task.UserId
        };
        _repository.AddTask(newTask);

        return newTask;
    }
    // public Task AddTask(Task task)
    // {
    //     var newTask = new SimpleTask
    //     {
    //         Title = task.Title,
    //         TaskDescription = task.TaskDescription,
    //         Priority = task.Priority,
    //         Status = task.Status,
    //         DueTime = task.DueTime,
    //         // UserId = task.UserId
    //     };
    //     _repository.AddTask(newTask);

    //     return newTask;
    // }
    public void DeleteTask(Task task)
    {
        _repository.DeleteTask(task);
    }
    public void UpdateTask(Task task)
    {
        _repository.UpdateTask(task);
    }
}
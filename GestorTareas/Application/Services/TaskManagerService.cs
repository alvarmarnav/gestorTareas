using System;
using Task = GestorTareas.Models.Task;
using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Enums;
using TaskStatus = GestorTareas.Enums.TaskStatus;
using CompositeTaskType = GestorTareas.Enums.CompositeTaskType;
using GestorTareas.Models;
using GestorTareas.Interfaces;

namespace GestorTareas.Application.Services;

public class TaskManagerService
{
    private readonly ITaskRepository _repository;

    public TaskManagerService(ITaskRepository repository) => _repository = repository;

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
        string title,
        string? taskDescription,
        TaskPriority? taskPriority,
        TaskStatus? taskStatus,
        DateTime? dueTime,
        CompositeTaskType? compositeTaskType,
        int? linkedTaskOrder,
        int? recurrenceRule,
        User? taskSupervisor)
    {


        Task newTask = compositeTaskType switch
        {
            // Casos con compositeTaskType != null
            not null when linkedTaskOrder is not null
                => new LinkedTask
                {
                    CompositeTaskType = compositeTaskType.Value,
                    LinkedTaskOrder = linkedTaskOrder
                },

            not null
                => new SubTask
                {
                    CompositeTaskType = compositeTaskType.Value
                },

            // Casos con compositeTaskType == null
            null when recurrenceRule is not null
                => new RecurringTask
                {
                    RecurrenceRule = recurrenceRule.Value
                },

            null when taskSupervisor is not null
                => new CollaborativeTask
                {
                    TaskSupervisor = taskSupervisor
                },

            null
                => new SimpleTask(),

            // _ => throw new ArgumentException("Error al introducir los parámetros")
        };

        newTask.Title = title;
        newTask.TaskDescription = taskDescription;
        newTask.Priority = taskPriority;
        newTask.Status = taskStatus;
        newTask.DueTime = dueTime;

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
using System;
using Task = GestorTareas.Models.Task;
using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Enums;
using TaskStatus = GestorTareas.Enums.TaskStatus;
using CompositeTaskType = GestorTareas.Enums.CompositeTaskType;
using GestorTareas.Models;
using GestorTareas.Interfaces;
using GestorTareas.Application.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using LinkedTask = GestorTareas.Models.LinkedTask;
using Microsoft.JSInterop;
using System.Security.Claims;
using claimUser = System.Security.Claims.ClaimsPrincipal;
using System.Collections.Immutable;

namespace GestorTareas.Application.Services;

public class TaskManagerService
{
    private readonly ITaskRepository _repository;

    public TaskManagerService(ITaskRepository repository) => _repository = repository;

    public List<Task> GetAllTasks()
    {
        return _repository.GetAllTasks();
    }
    public List<Task> GetAllTasksByUser(int userId)
    {
        return _repository.GetAllTasksByUser(userId);
    }
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
        int userId,
        string? taskDescription,
        TaskPriority? taskPriority,
        TaskStatus? taskStatus,
        DateTime? dueTime,
        string? cancelReason,
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


            _ => new SimpleTask(),

            // _ => throw new ArgumentException("Error al introducir los parámetros")
        };

        newTask.Title = title;
        newTask.UserId = userId;
        newTask.TaskDescription = taskDescription;
        newTask.Priority = taskPriority;
        newTask.Status = taskStatus;
        newTask.DueTime = dueTime;
        newTask.CancelReason = cancelReason;

        _repository.AddTask(newTask);

        return newTask;
    }

    public void DeleteTask(int id)
    {
        var task = _repository.GetTaskById(id) ?? throw new KeyNotFoundException($"No existe la tarea con ID: {id}");
        _repository.DeleteTask(task);
    }
    public void UpdateTask(int id, UpdateTaskDto taskDto)
    {//TODO: observar esta exception
        var updateTask = _repository.GetTaskById(id) ?? throw new Exception();

        switch (updateTask)
        {

            case LinkedTask linked:
                linked.LinkedTaskOrder = taskDto.LinkedTaskOrder ?? linked.LinkedTaskOrder;
                break;
            case RecurringTask recurring:

                recurring.RecurrenceRule = taskDto.RecurrenceRule ?? recurring.RecurrenceRule;
                break;

            case CollaborativeTask collab:
                collab.TaskSupervisor = taskDto.TaskSupervisor ?? collab.TaskSupervisor;
                break;

            default:
                break;
        }

        updateTask.Title = taskDto.Title ?? updateTask.Title;
        updateTask.TaskDescription = taskDto.TaskDescription ?? updateTask.TaskDescription;
        updateTask.Priority = taskDto.Priority ?? updateTask.Priority;
        updateTask.Status = taskDto.Status ?? updateTask.Status;
        updateTask.DueTime = taskDto.DueTime ?? updateTask.DueTime;

        _repository.UpdateTask(updateTask);
    }

    public PaginationResponseDto<ResponseTaskDto> GetPagination(int pageNumber, int itemsPerPage)
    {

        var (tasks, total) = _repository.GetTotalPaginated(pageNumber, itemsPerPage);

        return new PaginationResponseDto<ResponseTaskDto>
        {
            Data = tasks
        .Select(t => new ResponseTaskDto
        {
            Id = t.Id,
            Title = t.Title,
            TaskDescription = t.TaskDescription,
            TaskPriority = (TaskPriority)t.Priority,
            TaskStatus = (TaskStatus)t.Status,
            DueTime = (DateTime)t.DueTime,
            CancelReason = t.CancelReason
        })
        .ToList(),
            ActualPage = pageNumber,
            TotalItems = total,
            ItemsPerPage = itemsPerPage,
            TotalPages = (int)Math.Ceiling(
        total / (double)itemsPerPage)
        };
    }

}
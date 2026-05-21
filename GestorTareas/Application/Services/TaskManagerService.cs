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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using GestorTareas.Controllers;

namespace GestorTareas.Application.Services;

public class TaskManagerService
{
    private readonly ITaskRepository _repository;
    private readonly IUserRepository _userRepository;
    public TaskManagerService(ITaskRepository repository, IUserRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public List<ResponseTaskDto> GetAllTasks()
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
    public List<ResponseTaskDto> GetAllTasksByUser(int userId)
    {
        var validUser = _userRepository.GetUserById(userId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {userId}");

        return _repository.GetAllTasksByUser(validUser.Id)
        .Select(t => new ResponseTaskDto
        {
            Id = t.Id,
            Title = t.Title,
            TaskDescription = t.TaskDescription,
            TaskPriority = (TaskPriority)t.Priority,
            TaskStatus = (TaskStatus)t.Status,
            DueTime = (DateTime)t.DueTime,
            CancelReason = t.CancelReason
        }).ToList(); ;
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
    public ResponseTaskDto? GetTaskById(int id)
    {
        var task = _repository.GetTaskById(id) ?? throw new KeyNotFoundException($"No existe la tarea con ID: {id}.");
        return new ResponseTaskDto
        {
            Id = task.Id,
            Title = task.Title,
            UserId = task.UserId,
            TaskDescription = task.TaskDescription,
            TaskPriority = (TaskPriority)task.Priority,
            TaskStatus = (Enums.TaskStatus)task.Status,
            DueTime = (DateTime)task.DueTime,
            CancelReason = task.CancelReason
        };
    }
    public Task AddTask(
            string title,
            int userId,
            string? taskDescription,
            TaskPriority? taskPriority,
            TaskStatus? taskStatus,
            DateTime? dueTime,
            int? recurrenceRule,
            List<TaskCollaborator>? taskCollaborators,
            List<SubTask>? subTasks,
            int? parentCompositeTaskId,
            int? linkedTaskOrder,
            int? taskId,
            int? dependsOnTaskId
            )
    {

        //TODO::observar
        Task newTask;

        if (linkedTaskOrder is not null)
        {
            newTask = new LinkedTask
            {
                // newTask.Id = IdentityApiEndpointRouteBuilderExtensions,
                TaskId = (int)taskId,
                // newTask.Task=Task;
                DependsOnTaskId = (int)dependsOnTaskId,
                // newTask.DependsOn =dependsOn;
                LinkedTaskOrder = (int)linkedTaskOrder
            };
        }
        else if (recurrenceRule is not null){
            newTask = new RecurringTask
            {
                RecurrenceRule = (int)recurrenceRule
            };
        }
        else if (taskCollaborators is not null && taskCollaborators.Any()){
            newTask = new CollaborativeTask
            {
                TaskCollaborators = taskCollaborators
            };
        }
        else if (subTasks is not null && subTasks.Any()){
            newTask = new CompositeTask
            {
                SubTaskList = subTasks
            };
        }
        else if (parentCompositeTaskId is not null){

            var parentTask = _repository.GetTaskById((int)parentCompositeTaskId);
            newTask = new SubTask
            {
                ParentCompositeTaskId = (int)parentCompositeTaskId,
                ParentCompositeTask = (CompositeTask)parentTask
            };
        }
        else
            newTask = new SimpleTask();

        newTask.Title = title;
        newTask.UserId = userId;
        newTask.TaskDescription = taskDescription;
        newTask.Priority = taskPriority;
        newTask.Status = taskStatus;
        newTask.DueTime = dueTime;

        _repository.AddTask(newTask);

        return newTask;
    }

    public void DeleteTask(int id, int userActiveId)
    {
        var task = _repository.GetTaskById(id) ?? throw new KeyNotFoundException($"No existe la tarea con ID: {id}");
        var userActive = _userRepository.GetUserById(userActiveId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {userActiveId}");

        if (!(bool)userActive.IsAdmin && task.UserId != userActiveId && !task.UsersList.Any(u => u.Id == userActiveId))
            throw new UnauthorizedAccessException("No está autorizado para realizar esta operación");
        _repository.DeleteTask(task);
    }
    public void UpdateTask(int id, UpdateTaskDto taskDto, int userActiveId)
    {//TODO: observar esta exception
        var updateTask = _repository.GetTaskById(id) ?? throw new Exception();
        var userActive = _userRepository.GetUserById(userActiveId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {userActiveId}");

        if (!(bool)userActive.IsAdmin && updateTask.UserId != userActiveId && !updateTask.UsersList.Any(u => u.Id == userActiveId))
            throw new UnauthorizedAccessException("No está autorizado para realizar esta operación");
        switch (updateTask)
        {

            // case LinkedTask linked:
            //     linked.LinkedTaskOrder = taskDto.LinkedTaskOrder ?? linked.LinkedTaskOrder;
            //     break;
            case RecurringTask recurring:

                recurring.RecurrenceRule = taskDto.RecurrenceRule ?? recurring.RecurrenceRule;
                break;

            case CollaborativeTask collab:

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

    public PaginationResponseDto<ResponseTaskDto> GetPagination(int pageNumber, int itemsPerPage, int userId)
    {
        //Aqui debo incluir la identificacion del user para que solo las pueda ver el propietario
        var userActive = _userRepository.GetUserById(userId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {userId}");
        var (tasks, total) = _repository.GetTotalPaginated(pageNumber, itemsPerPage, userId);

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
    public void AddNewTeamMember(int taskId, int userId)
    {
        var selectedUser = _userRepository.GetUserById(userId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {userId}");
        var selectedTask = _repository.GetTaskById(taskId) ?? throw new KeyNotFoundException($"No existe ninguna Tarea con el ID: {taskId}");
        if (selectedTask.GetType().Name == "CollaborativeTask")
            _repository.AddNewTeamMember((CollaborativeTask)selectedTask, (TaskCollaborator)selectedUser);
        else
            throw new ArgumentException($"La tarea seleccionada es del tipo({selectedTask.GetType().Name}) no es del tipo colaborativo.");
    }
    public void RemoveTeamMember(int taskId, int userId)
    {
        var selectedUser = _userRepository.GetUserById(userId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {userId}");
        var selectedTask = _repository.GetTaskById(taskId) ?? throw new KeyNotFoundException($"No existe ninguna Tarea con el ID: {taskId}");
        if (selectedTask is CollaborativeTask colTask)
        {
            if (colTask.TaskCollaborators.Any(m => m.UserId == userId))
                throw new ArgumentException($"El usuario con ID({userId}) ya está en el equipo.");
            else
                _repository.RemoveTeamMember((CollaborativeTask)selectedTask, (TaskCollaborator)selectedUser);
        }
        else
            throw new ArgumentException($"La tarea seleccionada es del tipo({selectedTask.GetType().Name}) no es del tipo colaborativo.");
    }
}
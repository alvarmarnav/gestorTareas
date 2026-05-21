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
using NUnit.Framework.Constraints;

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
            DueTime = t.DueTime,
            CancelReason = t.CancelReason
        }).ToList();
    }
    public List<ResponseTaskDto> GetAllTasksByUser(int? userId, CurrentUserDto currentUserDto)
    {
        var validUser = _userRepository.GetUserById(currentUserDto.CurrentUserId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {currentUserDto.CurrentUserId}");
        if (userId is not null && userId != currentUserDto.CurrentUserId && currentUserDto.CurrentUserRole is not CollaboratorRole.Admin)
            throw new KeyNotFoundException($"No existe ningun usuario con el ID: {currentUserDto.CurrentUserId}");
        if (userId is null) userId = (int)currentUserDto.CurrentUserId;

        return _repository.GetAllTasksByUser(validUser.Id)
        .Select(t => new ResponseTaskDto
        {
            Id = t.Id,
            Title = t.Title,
            TaskDescription = t.TaskDescription,
            TaskPriority = (TaskPriority)t.Priority,
            TaskStatus = (TaskStatus)t.Status,
            DueTime = t.DueTime,
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
            DueTime = t.DueTime,
            CancelReason = t.CancelReason
        }).ToList();
    }
    public ResponseTaskDto? GetTaskById(int id, CurrentUserDto userDto)
    {
        var task = _repository.GetTaskById(id) ?? throw new KeyNotFoundException($"No existe la tarea con ID: {id}.");

        if (task.UserId != userDto.CurrentUserId && !task.UsersList.Any(u => u.Id == userDto.CurrentUserId) && userDto.CurrentUserRole != Enums.CollaboratorRole.Admin)
            throw new UnauthorizedAccessException($"Acceso denegado.");

        return new ResponseTaskDto
        {
            Id = task.Id,
            Title = task.Title,
            UserId = task.UserId,
            TaskDescription = task.TaskDescription,
            TaskPriority = (TaskPriority)task.Priority,
            TaskStatus = (Enums.TaskStatus)task.Status,
            DueTime = task.DueTime,
            CancelReason = task.CancelReason
        };
    }
    public Task CreateTask(
            string title,
            CurrentUserDto currentUserDto,
            string? taskDescription,
            TaskPriority? taskPriority,
            TaskStatus? taskStatus,
            DateTime? dueTime,
            int? recurrenceRule,
            List<TaskCollaborator>? taskCollaborators,
            List<SubTask>? subTasks,
            int? parentCompositeTaskId,
            int? taskId,
            int? linkedTaskOrder,
            int? dependsOnTaskId
            )
    {

        //TODO::observar
        Task newTask;

        if (recurrenceRule is not null)
        {
            newTask = new RecurringTask
            {
                RecurrenceRule = (int)recurrenceRule
            };
        }
        else if (taskCollaborators is not null && taskCollaborators.Any())
        {
            newTask = new CollaborativeTask
            {
                TaskCollaborators = taskCollaborators
            };
        }
        else if (subTasks is not null && subTasks.Any())
        {
            newTask = new CompositeTask
            {
                SubTaskList = subTasks
            };
        }
        else if (parentCompositeTaskId is not null)
        {

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
        newTask.UserId = currentUserDto.CurrentUserId;
        newTask.TaskDescription = taskDescription;
        newTask.Priority = taskPriority;
        newTask.Status = taskStatus;
        newTask.DueTime = dueTime;

        if (newTask.DueTime.HasValue && newTask.DueTime.Value <= DateTime.UtcNow)
        {
            throw new ArgumentException("La fecha de vencimiento debe ser futura.");
        }
        else if (newTask.DueTime.HasValue && newTask.DueTime.Value > DateTime.UtcNow.AddYears(2))
        {
            throw new ArgumentException("La fecha de vencimiento No debe ser mayor a 2 años.");
        }



        _repository.CreateTask(newTask);
        if (linkedTaskOrder is not null)
            this.AddLinkedTask(newTask.Id, dependsOnTaskId, linkedTaskOrder, currentUserDto);

        return newTask;
    }

    public LinkedTask AddLinkedTask(int taskId, int? dependsOnTaskId, int? linkedTaskOrder, CurrentUserDto currentUserDto)
    {
        if (currentUserDto is null)
            throw new UnauthorizedAccessException("Acceso no permitido.");
        if (linkedTaskOrder <= 0)//TODO::Aqui logica para si no pone nada poner la ultima y si no pone nada y es la primera ponerla en la 0
            throw new ArgumentException($"La posición no puede ser inferior a 0.");
        if (taskId == dependsOnTaskId)
            throw new ArgumentException($"Una tarea no debe depende de sí misma.");

        var taskTarget = _repository.GetTaskById(taskId) ?? throw new KeyNotFoundException($"No existe ninguna tarea con el ID: {taskId}.");

        if (currentUserDto.CurrentUserRole is not CollaboratorRole.Admin && currentUserDto.CurrentUserId != taskTarget.UserId)
            throw new UnauthorizedAccessException("Acceso no autorizado.");
        if (dependsOnTaskId is not null)
        {
            var dependsOnTask = _repository.GetTaskById(dependsOnTaskId.Value);

            if (dependsOnTask is null)
                throw new KeyNotFoundException($"No existe ninguna tarea con el ID: {dependsOnTaskId}.");

            if (dependsOnTask.UserId != currentUserDto.CurrentUserId && currentUserDto.CurrentUserRole is not CollaboratorRole.Admin)
                throw new UnauthorizedAccessException("Acceso no autorizado.");
        }
        if (_repository.ExistsRecurrenceRelation(taskId, (int)dependsOnTaskId))
            throw new InvalidOperationException("No es posible añadir esta relacion recursiva.");

        if (_repository.ExistsLinkedRelation(taskId, (int)dependsOnTaskId))
            throw new InvalidOperationException("No es posible añadir esta relacion porque ya existe.");


        var linkedTask = new LinkedTask
        {
            TaskId = taskId,
            DependsOnTaskId = (int)dependsOnTaskId,
            LinkedTaskOrder = (int)linkedTaskOrder
        };
        return _repository.AddLinkedRelation(linkedTask);
    }

    public void DeleteTask(int id, CurrentUserDto currentUserDto)
    {
        var task = _repository.GetTaskById(id) ?? throw new KeyNotFoundException($"No existe la tarea con ID: {id}");
        var userActive = _userRepository.GetUserById(currentUserDto.CurrentUserId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {currentUserDto.CurrentUserId}");

        if (!(bool)userActive.IsAdmin && task.UserId != currentUserDto.CurrentUserId && !task.UsersList.Any(u => u.Id == currentUserDto.CurrentUserId))
            throw new UnauthorizedAccessException("No está autorizado para realizar esta operación");
        _repository.DeleteTask(task);
    }
    public void UpdateTask(int id, UpdateTaskDto taskDto, CurrentUserDto currentUserDto)
    {//TODO: observar esta exception
        var updateTask = _repository.GetTaskById(id) ?? throw new Exception();
        var userActive = _userRepository.GetUserById(currentUserDto.CurrentUserId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {currentUserDto.CurrentUserId}");

        if (!(bool)userActive.IsAdmin && updateTask.UserId != currentUserDto.CurrentUserId && !updateTask.UsersList.Any(u => u.Id == currentUserDto.CurrentUserId))
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

    public PaginationResponseDto<ResponseTaskDto> GetPagination(int pageNumber, int itemsPerPage, CurrentUserDto currentUserDto)
    {
        //Aqui debo incluir la identificacion del user para que solo las pueda ver el propietario
        var userActive = _userRepository.GetUserById(currentUserDto.CurrentUserId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {currentUserDto.CurrentUserId}");
        var (tasks, total) = _repository.GetTotalPaginated(pageNumber, itemsPerPage, currentUserDto.CurrentUserId);

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
            DueTime = t.DueTime,
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
    public void AddTaskCollaborator(int taskId, CreateTaskCollaboratorDto createTaskCollaboratorDto)
    {
        var selectedUser = _userRepository.GetUserById((int)createTaskCollaboratorDto.UserId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {createTaskCollaboratorDto}");
        var selectedTask = _repository.GetTaskById(taskId) ?? throw new KeyNotFoundException($"No existe ninguna Tarea con el ID: {taskId}");
        if (selectedTask.GetType().Name != "CollaborativeTask")
            throw new ArgumentException($"La tarea seleccionada es del tipo({selectedTask.GetType().Name}) no es del tipo colaborativo.");

        if (selectedTask.UsersList.Any(m => m.Id == createTaskCollaboratorDto.UserId))
            throw new ArgumentException($"El usuario con ID({createTaskCollaboratorDto.UserId}) ya está en el equipo.");
        TaskCollaborator taskCollaborator = new TaskCollaborator
        {
            UserId = selectedUser.Id,
            UserTask = selectedUser,
            TaskId = selectedTask.Id,
            Task = (CollaborativeTask)selectedTask,
            CollaboratorRole = CollaboratorRole.Collaborator,
            AddedAt = DateTime.UtcNow
        };
        _repository.AddTaskCollaborator((CollaborativeTask)selectedTask, taskCollaborator);

    }
    public void RemoveTaskCollaborator(int taskId, RemoveTaskCollaboratorDto removeTaskCollaboratorDto)
    {
        var selectedUser = _userRepository.GetUserById(removeTaskCollaboratorDto.UserId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {removeTaskCollaboratorDto.UserId}");
        var selectedTask = _repository.GetTaskById(taskId) ?? throw new KeyNotFoundException($"No existe ninguna Tarea con el ID: {taskId}");
        if (selectedTask is not CollaborativeTask colTask)
            throw new ArgumentException($"La tarea seleccionada es del tipo({selectedTask.GetType().Name}) no es del tipo colaborativo.");

        if (!colTask.TaskCollaborators.Any(m => m.UserId == removeTaskCollaboratorDto.UserId))
            throw new ArgumentException($"El usuario con ID({removeTaskCollaboratorDto.UserId}) NO está en el equipo.");
        TaskCollaborator taskCollaborator = new TaskCollaborator
        {
            UserId = selectedUser.Id,
            UserTask = selectedUser,
            TaskId = selectedTask.Id,
            Task = (CollaborativeTask)selectedTask,
            CollaboratorRole = CollaboratorRole.Collaborator,
            AddedAt = DateTime.UtcNow
        };
        _repository.RemoveTaskCollaborator((CollaborativeTask)selectedTask, taskCollaborator);

    }
}
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
    public List<ResponseTaskDto> GetAllTasksByUser(int userId, CurrentUserDto currentUserDto)
    {
        if (userId <= 0) throw new ArgumentException("No se ha introducido valor de búsqueda.");
        var validUser = _userRepository.GetUserById(currentUserDto.CurrentUserId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {currentUserDto.CurrentUserId}");
        if (userId != currentUserDto.CurrentUserId && currentUserDto.CurrentUserSystemRole is not SystemRole.Admin)
            throw new KeyNotFoundException($"No existe ningun usuario con el ID: {currentUserDto.CurrentUserId}");

        return _repository.GetAllTasksByUser(userId)
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
    public List<ResponseTaskDto> GetAllTaskOwnUser(CurrentUserDto currentUserDto)
    {
        var validUser = _userRepository.GetUserById(currentUserDto.CurrentUserId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {currentUserDto.CurrentUserId}");
        // if (currentUserDto.CurrentUserRole is CollaboratorRole.Admin)
        //     throw new KeyNotFoundException($"No existe ningun usuario con el ID: {currentUserDto.CurrentUserId}");

        return _repository.GetAllTasksByUser(currentUserDto.CurrentUserId)
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

        if (task.UserId != userDto.CurrentUserId && !task.UsersList.Any(u => u.Id == userDto.CurrentUserId) && userDto.CurrentUserSystemRole != Enums.SystemRole.Admin)
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
    // public TaskDTO CreateTask(CreateSimpleTaskDto dto, TaskType taskType, CurrentUserDto userDto)
    // {

    //     if (userDto is null) throw new UnauthorizedAccessException("Usuario No autorizado.");

    //     var newTask = new SimpleTask
    //     {
    //         Title = dto.Title,
    //         UserId = userDto.CurrentUserId,
    //         TaskDescription = dto.TaskDescription,
    //         Priority = dto.Priority,
    //         DueTime = dto.DueTime,
    //     };

    //     if (newTask.DueTime.HasValue && newTask.DueTime.Value <= DateTime.UtcNow)
    //     {
    //         throw new ArgumentException("La fecha de vencimiento debe ser futura.");
    //     }
    //     else if (newTask.DueTime.HasValue && newTask.DueTime.Value > DateTime.UtcNow.AddYears(2))
    //     {
    //         throw new ArgumentException("La fecha de vencimiento No debe ser mayor a 2 años.");
    //     }

    //     var createdTask = _repository.CreateTask(newTask);

    //     return DtoManager.TaskToDto(createdTask);
    // }
    public TaskDTO CreateTask(CreateSimpleTaskDto dto, CurrentUserDto userDto)
    {

        if (userDto is null) throw new UnauthorizedAccessException("Usuario No autorizado.");

        var newTask = new SimpleTask
        {
            Title = dto.Title,
            UserId = userDto.CurrentUserId,
            TaskDescription = dto.TaskDescription,
            Priority = dto.Priority,
            DueTime = dto.DueTime,
        };

        if (newTask.DueTime.HasValue && newTask.DueTime.Value <= DateTime.UtcNow)
        {
            throw new ArgumentException("La fecha de vencimiento debe ser futura.");
        }
        else if (newTask.DueTime.HasValue && newTask.DueTime.Value > DateTime.UtcNow.AddYears(2))
        {
            throw new ArgumentException("La fecha de vencimiento No debe ser mayor a 2 años.");
        }

        var createdTask = _repository.CreateTask(newTask);

        return DtoManager.TaskToDto(createdTask);
    }
    public ResponseRecurringTaskDto CreateRecurringTask(CreateRecurringTaskDto dto, CurrentUserDto userDto)
    {

        if (userDto is null) throw new UnauthorizedAccessException("Usuario No autorizado.");

        var newTask = new RecurringTask
        {
            Title = dto.Title,
            UserId = userDto.CurrentUserId,
            DueTime = dto.DueTime,
            RecurrenceRule = dto.RecurrenceRule,
            TaskDescription = dto.TaskDescription,
            Priority = dto.Priority,
        };

        if (newTask.DueTime.HasValue && newTask.DueTime.Value <= DateTime.UtcNow)
        {
            throw new ArgumentException("La fecha de vencimiento debe ser futura.");
        }
        else if (newTask.DueTime.HasValue && newTask.DueTime.Value > DateTime.UtcNow.AddYears(2))
        {
            throw new ArgumentException("La fecha de vencimiento No debe ser mayor a 2 años.");
        }

        var createdTask = _repository.CreateTask(newTask);

        var dtoReturned = DtoManager.TaskToDto(createdTask);

        var responseDto = new ResponseRecurringTaskDto
        {
            Title = dtoReturned.Title,
            UserId = userDto.CurrentUserId,
            TaskDescription = dtoReturned.TaskDescription,
            Priority = dtoReturned.Priority,
            DueTime = dtoReturned.DueTime,
            RecurrenceRule = (int)dtoReturned.RecurrenceRule,
            //  RecurringTasksCount= dtoReturned.recurringTaskCount,
        };
        return responseDto;
    }

    public TaskDTO CreateCollaborativeTask(CreateCollaborativeTaskDto dto, CurrentUserDto userDto)
    {

        if (userDto is null) throw new UnauthorizedAccessException("Usuario No autorizado.");

        var newTask = new CollaborativeTask
        {
            Title = dto.Title,
            UserId = userDto.CurrentUserId,
            TaskDescription = dto.TaskDescription,
            Priority = dto.Priority,
            DueTime = dto.DueTime,
        };


        if (newTask.DueTime.HasValue && newTask.DueTime.Value <= DateTime.UtcNow)
        {
            throw new ArgumentException("La fecha de vencimiento debe ser futura.");
        }
        else if (newTask.DueTime.HasValue && newTask.DueTime.Value > DateTime.UtcNow.AddYears(2))
        {
            throw new ArgumentException("La fecha de vencimiento No debe ser mayor a 2 años.");
        }

        var createdTask = _repository.CreateTask(newTask);

        var currentUserCollaborator = new CreateTaskCollaboratorDto
        {
            UserId = userDto.CurrentUserId,
            CollaboratorRole = CollaboratorRole.TaskAdministrator
        };

        this.AddTaskCollaborator(createdTask.Id, currentUserCollaborator, userDto);

        return DtoManager.TaskToDto(createdTask);
    }
    public TaskDTO CreateCompositeTask(CreateCompositeTaskDto dto, CurrentUserDto userDto)
    {

        if (userDto is null) throw new UnauthorizedAccessException("Usuario No autorizado.");

        var newTask = new CompositeTask
        {
            Title = dto.Title,
            UserId = userDto.CurrentUserId,
            TaskDescription = dto.TaskDescription,
            Priority = (TaskPriority?)dto.Priority,
            DueTime = dto.DueTime,
        };


        if (newTask.DueTime.HasValue && newTask.DueTime.Value <= DateTime.UtcNow)
        {
            throw new ArgumentException("La fecha de vencimiento debe ser futura.");
        }
        else if (newTask.DueTime.HasValue && newTask.DueTime.Value > DateTime.UtcNow.AddYears(2))
        {
            throw new ArgumentException("La fecha de vencimiento No debe ser mayor a 2 años.");
        }

        var createdTask = _repository.CreateTask(newTask);

        return DtoManager.TaskToDto(createdTask);
    }

    public TaskDTO CreateSubTask(int compositeTaskId, CreateSubTaskDto dto, CurrentUserDto userDto)
    {

        if (userDto is null) throw new UnauthorizedAccessException("Usuario No autorizado.");

        var compositeTask = _repository.GetTaskById(compositeTaskId) ?? throw new KeyNotFoundException($"No existe Tarea Compuesta con ID: {compositeTaskId}.");
        if (compositeTask is not CompositeTask parent) throw new InvalidOperationException("La tarea a la que se quiere vincular la SubTarea NO es del tipo correcto.");

        if (compositeTask.UserId != userDto.CurrentUserId) throw new InvalidOperationException("LA tarea no te pertenece y no puedes añadirle subtareas.");

        var newTask = new SubTask
        {
            Title = dto.Title,
            UserId = userDto.CurrentUserId,
            TaskDescription = dto.TaskDescription,
            Priority = dto.Priority,
            DueTime = dto.DueTime,
            ParentCompositeTaskId = compositeTaskId
        };

        if (newTask.DueTime.HasValue && newTask.DueTime.Value <= DateTime.UtcNow)
        {
            throw new ArgumentException("La fecha de vencimiento debe ser futura.");
        }
        else if (newTask.DueTime.HasValue && newTask.DueTime.Value > DateTime.UtcNow.AddYears(2))
        {
            throw new ArgumentException("La fecha de vencimiento No debe ser mayor a 2 años.");
        }

        var createdTask = _repository.CreateTask(newTask);

        // _repository.UpdateCompositeTask(compositeTaskId, (SubTask)createdTask);

        return DtoManager.TaskToDto(createdTask);
    }
    public ResponseLinkedTaskDto AddLinkedTask(int taskId, int dependsOnTaskId, int linkedTaskOrder, CurrentUserDto currentUserDto)
    {
        if (currentUserDto is null)
            throw new UnauthorizedAccessException("Acceso no permitido.");
        if (linkedTaskOrder <= 0)//TODO::Aqui logica para si no pone nada poner la ultima y si no pone nada y es la primera ponerla en la 0
            throw new ArgumentException($"La posición no puede ser inferior a 0.");
        if (taskId <= 0 || dependsOnTaskId <= 0)//TODO::Aqui logica para si no pone nada poner la ultima y si no pone nada y es la primera ponerla en la 0
            throw new ArgumentException($"La Id no puede ser inferior a 0.");
        if (taskId == dependsOnTaskId)
            throw new ArgumentException($"Una tarea no debe depende de sí misma.");

        var taskTarget = _repository.GetTaskById(taskId) ?? throw new KeyNotFoundException($"No existe ninguna tarea con el ID: {taskId}.");

        // if (currentUserDto.CurrentUserSystemRole is not SystemRole.Admin && currentUserDto.CurrentUserId != taskTarget.UserId)
        //     throw new UnauthorizedAccessException("Acceso no autorizado.");
        // // if (dependsOnTaskId is not null)
        // {
        var dependsOnTask = _repository.GetTaskById(dependsOnTaskId);

        if (dependsOnTask is null)
            throw new KeyNotFoundException($"No existe ninguna tarea con el ID: {dependsOnTaskId}.");

        if (dependsOnTask.UserId != currentUserDto.CurrentUserId)
            throw new UnauthorizedAccessException("Acceso no autorizado.");
        // }
        if (taskTarget.UserId != dependsOnTask.UserId)
            throw new InvalidOperationException("No es posible añadir relación entre tareas de distinto usuario.");

        if (_repository.ExistsCircularRelation(taskId, dependsOnTaskId))
            throw new InvalidOperationException("No es posible añadir esta relacion recursiva.");

        if (_repository.ExistsLinkedRelation(taskId, dependsOnTaskId))
            throw new InvalidOperationException("No es posible añadir esta relacion porque ya existe.");


        var linkedTask = new LinkedTask
        {
            TaskId = taskId,
            DependsOnTaskId = dependsOnTaskId,
            LinkedTaskOrder = linkedTaskOrder
        };
        var ltCreated = _repository.AddLinkedRelation(linkedTask);

        var linkedTaskDto = new ResponseLinkedTaskDto
        {
            Id = ltCreated.Id,
            TaskId = ltCreated.TaskId,
            DependsOnTaskId = ltCreated.DependsOnTaskId,
            LinkedTaskOrder = ltCreated.LinkedTaskOrder??0,
        };
        return linkedTaskDto;
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

        if (pageNumber < 1) pageNumber = 1;
        if (itemsPerPage < 1) itemsPerPage = 10;

        var userActive = _userRepository.GetUserById(currentUserDto.CurrentUserId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {currentUserDto.CurrentUserId}");
        var (taskQuery, total) = _repository.GetTotalPaginated(pageNumber, itemsPerPage, currentUserDto.CurrentUserId);

        return new PaginationResponseDto<ResponseTaskDto>
        {
            Data = taskQuery
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
    public void AddTaskCollaborator(int taskId, CreateTaskCollaboratorDto createTaskCollaboratorDto, CurrentUserDto currentUserDto)
    {
        var selectedUser = _userRepository.GetUserById((int)createTaskCollaboratorDto.UserId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {createTaskCollaboratorDto}");

        var selectedTask = _repository.GetTaskById(taskId) ?? throw new KeyNotFoundException($"No existe ninguna Tarea con el ID: {taskId}");

        if (!(bool)selectedUser.IsAdmin && selectedTask.UserId != currentUserDto.CurrentUserId && !selectedTask.UsersList.Any(u => u.Id == currentUserDto.CurrentUserId))
            throw new UnauthorizedAccessException("No está autorizado para realizar esta operación");

        if (selectedTask is not CollaborativeTask collaborativeTaskSelected)
            throw new ArgumentException($"La tarea seleccionada es del tipo({selectedTask.GetType().Name}) no es del tipo colaborativo.");

        ValidateCanAddCollaborator(collaborativeTaskSelected, currentUserDto);

        if (selectedTask.UsersList.Any(m => m.Id == createTaskCollaboratorDto.UserId))
            throw new ArgumentException($"El usuario con ID({createTaskCollaboratorDto.UserId}) ya está en el equipo.");
        TaskCollaborator taskCollaborator = new TaskCollaborator
        {
            UserId = selectedUser.Id,
            UserTask = selectedUser,
            TaskId = selectedTask.Id,
            Task = collaborativeTaskSelected,
            CollaboratorRole = createTaskCollaboratorDto.CollaboratorRole,
            AddedAt = DateTime.UtcNow
        };
        _repository.AddTaskCollaborator(collaborativeTaskSelected, taskCollaborator);

    }
    private void ValidateCanAddCollaborator(CollaborativeTask selectedTask, CurrentUserDto currentUserDto)
    {
        var isAdmin = currentUserDto.CurrentUserSystemRole == SystemRole.Admin;
        var isOwner = selectedTask.UserId == currentUserDto.CurrentUserId;
        var isTaskAdmin = selectedTask.TaskCollaborators.Any(tc =>
            tc.UserId == currentUserDto.CurrentUserId &&
            tc.CollaboratorRole == CollaboratorRole.TaskAdministrator);

        if (!isAdmin && !isOwner && !isTaskAdmin)
            throw new UnauthorizedAccessException("No tienes permisos para gestionar colaboradores en la tarea.");
    }

    public void RemoveTaskCollaborator(int taskId, RemoveTaskCollaboratorDto removeTaskCollaboratorDto, CurrentUserDto currentUserDto)
    {
        var selectedUser = _userRepository.GetUserById(removeTaskCollaboratorDto.UserId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {removeTaskCollaboratorDto.UserId}");

        var selectedTask = _repository.GetTaskById(taskId) ?? throw new KeyNotFoundException($"No existe ninguna Tarea con el ID: {taskId}");

        if (selectedTask is not CollaborativeTask colTask)
            throw new ArgumentException($"La tarea seleccionada es del tipo({selectedTask.GetType().Name}) no es del tipo colaborativo.");

        ValidateCanAddCollaborator(colTask, currentUserDto);


        // if (!colTask.TaskCollaborators.Any(m => m.UserId == selectedUser.Id))
        //     throw new ArgumentException($"El usuario con ID({selectedUser.Id}) NO está en el equipo.");

var taskCollaborator = colTask.TaskCollaborators.FirstOrDefault(tc => tc.UserId ==removeTaskCollaboratorDto.UserId)?? throw new KeyNotFoundException($"El usuario con ID({removeTaskCollaboratorDto.UserId}) NO está en el equipo.");

        
        _repository.RemoveTaskCollaborator(colTask, taskCollaborator);

    }


}
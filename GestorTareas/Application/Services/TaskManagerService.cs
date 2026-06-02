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
using Microsoft.VisualBasic;
using System.Windows.Markup;

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
            TaskPriority = (Priority)t.TaskPriority,
            TaskStatus = (TaskStatus)t.TaskStatus,
            DueTime = t.DueTime,
            CancelReason = t.CancelReason
        }).ToList();
    }
    public List<ResponseTaskDto> GetAllTasksByUser(int userId, CurrentUserDto currentUserDto)
    {
        if (userId <= 0) throw new ArgumentException("No se ha introducido valor de búsqueda.");
        var validUser = EnsureActiveUser(currentUserDto);
        if (userId != currentUserDto.CurrentUserId && currentUserDto.CurrentUserSystemRole is not SystemRole.Admin)
            throw new KeyNotFoundException($"No existe ningun usuario con el ID: {currentUserDto.CurrentUserId}");

        return _repository.GetAllTasksByUser(userId)
        .Select(t => new ResponseTaskDto
        {
            Id = t.Id,
            Title = t.Title,
            TaskDescription = t.TaskDescription,
            TaskPriority = (Priority)t.TaskPriority,
            TaskStatus = (TaskStatus)t.TaskStatus,
            DueTime = t.DueTime,
            CancelReason = t.CancelReason
        }).ToList(); ;
    }
    public List<ResponseTaskDto> GetAllTaskOwnUser(CurrentUserDto currentUserDto)
    {
        var validUser = EnsureActiveUser(currentUserDto);

        return _repository.GetAllTasksByUser(currentUserDto.CurrentUserId)
        .Select(t => new ResponseTaskDto
        {
            Id = t.Id,
            Title = t.Title,
            TaskDescription = t.TaskDescription,
            TaskPriority = (Priority)t.TaskPriority,
            TaskStatus = (TaskStatus)t.TaskStatus,
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
            TaskPriority = (Priority)t.TaskPriority,
            TaskStatus = (TaskStatus)t.TaskStatus,
            DueTime = t.DueTime,
            CancelReason = t.CancelReason
        }).ToList();
    }
    public ResponseTaskDto? GetTaskById(int id, CurrentUserDto userDto)
    {
        var task = _repository.GetTaskById(id) ?? throw new KeyNotFoundException($"No existe la tarea con ID: {id}.");

        ValidateCanEditTask(task, userDto);

        return new ResponseTaskDto
        {
            Id = task.Id,
            Title = task.Title,
            UserId = task.UserId,
            TaskDescription = task.TaskDescription,
            TaskPriority = (Priority)task.TaskPriority,
            TaskStatus = (Enums.TaskStatus)task.TaskStatus,
            DueTime = task.DueTime,
            CancelReason = task.CancelReason
        };
    }
    
    public TaskDTO CreateTask(CreateSimpleTaskDto dto, CurrentUserDto userDto)
    {

        if (userDto is null) throw new UnauthorizedAccessException("Usuario No autorizado.");

        var newTask = new SimpleTask
        {
            Title = dto.Title,
            UserId = userDto.CurrentUserId,
            TaskDescription = dto.TaskDescription,
            TaskPriority = dto.TaskPriority ?? Priority.Normal,
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
    public List<ResponseRecurringTaskDto> CreateRecurringTask(CreateRecurringTaskDto dto, CurrentUserDto userDto)
    {

        if (userDto is null) throw new UnauthorizedAccessException("Usuario No autorizado.");
        EnsureActiveUser(userDto);
        ValidateDueTime(dto.DueTime);

        if (dto.RepeatUntilDate == default) throw new ArgumentException("La fecha final de las repeticiones es obligatoria.");
        if (dto.RepeatUntilDate < dto.DueTime) throw new ArgumentException("La fecha final debe ser posterior a la inicial.");
        if (dto.RecurrenceRule <= 0 || dto.RecurrenceRule > 365) throw new ArgumentException("El valor de itaración debe estar entre 1 y 365 días.");
        if (dto.MaxOcurrences <= 0 || dto.MaxOcurrences > 100) throw new ArgumentException("Número de ocurrencias no válido.");

        var responseOcurrences = new List<ResponseRecurringTaskDto>();
        var dueTime = dto.DueTime;
        var count = 0;
        var seriesId = Guid.NewGuid();


        while (dueTime <= dto.RepeatUntilDate && count < dto.MaxOcurrences)
        {
            var taskOcurrence = new RecurringTask
            {
                Title = dto.Title,
                UserId = userDto.CurrentUserId,
                TaskDescription = dto.TaskDescription,
                TaskPriority = dto.TaskPriority,
                TaskType = TaskType.RecurringTask,
                DueTime = dueTime,
                RecurrenceRule = (int)dto.RecurrenceRule,
                RecurringTasksCount = count,
                RecurringSeriesId = seriesId,
            };
            var createdTask = _repository.CreateTask(taskOcurrence);
            responseOcurrences.Add((ResponseRecurringTaskDto)DtoManager.TaskToDto(createdTask));
            dueTime = dueTime.AddDays(dto.RecurrenceRule);
            count++;
        }
        if (responseOcurrences.Count == 0) throw new InvalidOperationException("No existe ninguna iteración recurrente.");
        return responseOcurrences;
    }

    public TaskDTO CreateCollaborativeTask(CreateCollaborativeTaskDto dto, CurrentUserDto userDto)
    {

        if (userDto is null) throw new UnauthorizedAccessException("Usuario No autorizado.");


        var newTask = new CollaborativeTask
        {
            Title = dto.Title,
            UserId = userDto.CurrentUserId,
            TaskDescription = dto.TaskDescription,
            TaskPriority = dto.TaskPriority ?? Priority.Normal,
            DueTime = dto.DueTime ?? null,
            TaskType = TaskType.CollaborativeTask,
            TaskCollaborators = new List<TaskCollaborator>
            {
                new TaskCollaborator
                {
                    UserId=userDto.CurrentUserId,
                    CollaboratorRole=CollaboratorRole.TaskAdministrator,
                }
            }
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
    public TaskDTO CreateCompositeTask(CreateCompositeTaskDto dto, CurrentUserDto userDto)
    {

        if (userDto is null) throw new UnauthorizedAccessException("Usuario No autorizado.");

        var newTask = new CompositeTask
        {
            Title = dto.Title,
            UserId = userDto.CurrentUserId,
            TaskDescription = dto.TaskDescription,
            TaskPriority = dto.TaskPriority ?? Priority.Normal,
            DueTime = dto.DueTime,
            TaskType = TaskType.CompositeTask,
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
        EnsureActiveUser(userDto);
        var compositeTask = _repository.GetTaskById(compositeTaskId) ?? throw new KeyNotFoundException($"No existe Tarea Compuesta con ID: {compositeTaskId}.");
        ValidateCanEditTask(compositeTask, userDto);
        if (compositeTask is not CompositeTask parent) throw new InvalidOperationException("La tarea a la que se quiere vincular la SubTarea NO es del tipo correcto.");

        if (compositeTask.UserId != userDto.CurrentUserId) throw new InvalidOperationException("LA tarea no te pertenece y no puedes añadirle subtareas.");
        if (parent.SubTaskList.Count >= CompositeTask.MaxSubTasks) throw new InvalidOperationException($"No se pueden añadir más de {CompositeTask.MaxSubTasks} subtareas.");
        if (dto.DueTime.HasValue)
        {
            ValidateDueTime(dto.DueTime);
            if (compositeTask.DueTime.HasValue && dto.DueTime.Value > compositeTask.DueTime.Value) throw new ArgumentException("La subtarea no puede tener una fecha fin posterior a la tarea compuesta.");
        }

        var newTask = new SubTask
        {
            Title = dto.Title,
            UserId = userDto.CurrentUserId,
            TaskDescription = dto.TaskDescription,
            TaskPriority = dto.TaskPriority ?? Priority.Normal,
            DueTime = dto.DueTime,
            ParentCompositeTaskId = compositeTaskId,
            TaskType = TaskType.SubTask,
        };

        var createdTask = _repository.CreateTask(newTask);
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
        var dependsOnTask = _repository.GetTaskById(dependsOnTaskId);

        if (dependsOnTask is null)
            throw new KeyNotFoundException($"No existe ninguna tarea con el ID: {dependsOnTaskId}.");


        ValidateCanEditTask(taskTarget, currentUserDto);
        ValidateCanEditTask(dependsOnTask, currentUserDto);

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
            LinkedTaskOrder = ltCreated.LinkedTaskOrder ?? 0,
        };
        return linkedTaskDto;
    }

    public void DeleteTask(int id, CurrentUserDto currentUserDto)
    {
        var task = _repository.GetTaskById(id) ?? throw new KeyNotFoundException($"No existe la tarea con ID: {id}");
        var userActive = EnsureActiveUser(currentUserDto);

        var isAdmin = currentUserDto.CurrentUserSystemRole == SystemRole.Admin;
        var isOwner = task.UserId == currentUserDto.CurrentUserId;

        //no admin no owner pero puede ser colaborador
        if (!isAdmin && !isOwner)
        {
            if (task is not CollaborativeTask collaborativeTask)
                throw new ArgumentException($"La tarea seleccionada es del tipo({task.GetType().Name}) no es del tipo colaborativo.");

            var collaborator = _repository.GetAllTaskCollaborators(task.Id, currentUserDto.CurrentUserId) ?? throw new UnauthorizedAccessException("No tiene permiso.");
            _repository.RemoveTaskCollaborator(collaborativeTask, collaborator);
            return;
        }

        var hasDependencies = task.Dependencies.Any();
        var isRequiredByOhers = task.RequiredByOtherTask.Any();

        if (hasDependencies || isRequiredByOhers) throw new InvalidOperationException("No es posible eliminar la tarea, está vinculada a otras.");

        _repository.DeleteTask(task);
    }
    public void UpdateTask(int id, UpdateTaskDto taskDto, CurrentUserDto currentUserDto)
    {
        var updateTask = _repository.GetTaskById(id) ?? throw new Exception();
        var userActive = EnsureActiveUser(currentUserDto);
        ValidateCanEditTask(updateTask, currentUserDto);
        switch (updateTask)
        {

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
        updateTask.TaskPriority = taskDto.TaskPriority ?? updateTask.TaskPriority;
        updateTask.DueTime = taskDto.DueTime ?? updateTask.DueTime;

        _repository.UpdateTask(updateTask);
    }

    public PaginationResponseDto<ResponseTaskDto> GetPagination(int actualPage, int itemsPerPage, CurrentUserDto currentUserDto)
    {

        if (actualPage < 1) actualPage = 1;
        if (itemsPerPage < 1) itemsPerPage = 10;

        var userActive = EnsureActiveUser(currentUserDto);
        var (taskQuery, total) = _repository.GetTotalPaginated(actualPage, itemsPerPage, currentUserDto.CurrentUserId);

        return new PaginationResponseDto<ResponseTaskDto>
        {
            Data = taskQuery
        .Select(t => new ResponseTaskDto
        {
            Id = t.Id,
            Title = t.Title,
            TaskDescription = t.TaskDescription,
            TaskType = t.TaskType,
            TaskPriority = (Priority)t.TaskPriority,
            TaskStatus = (TaskStatus)t.TaskStatus,
            DueTime = t.DueTime,
            CancelReason = t.CancelReason
        })
        .ToList(),
            ActualPage = actualPage,
            TotalItems = total,
            ItemsPerPage = itemsPerPage,
            TotalPages = (int)Math.Ceiling(
        total / (double)itemsPerPage)
        };
    }
    public void AddTaskCollaborator(int taskId, CreateTaskCollaboratorDto createTaskCollaboratorDto, CurrentUserDto currentUserDto)
    {
        var selectedTask = _repository.GetTaskById(taskId) ?? throw new KeyNotFoundException($"No existe ninguna Tarea con el ID: {taskId}");
        if (!(selectedTask is CollaborativeTask collabTask)) throw new ArgumentException($"La tarea seleccionada es del tipo({selectedTask.GetType().Name}) no es del tipo colaborativo.");

        ValidateCanManageCollaborators(collabTask, currentUserDto);
        if (_repository.AlreadyExistsCollaborator(taskId, createTaskCollaboratorDto.UserId)) throw new InvalidOperationException("El usuario ya es colaborador.");

        User user = _userRepository.GetUserById(createTaskCollaboratorDto.UserId) ?? throw new KeyNotFoundException("No existe el usuario.");

        TaskCollaborator taskCollaborator = new TaskCollaborator
        {
            UserId = user.Id,
            UserTask = user,
            TaskId = collabTask.Id,
            Task = collabTask,
            CollaboratorRole = createTaskCollaboratorDto.CollaboratorRole,
            AddedAt = DateTime.UtcNow
        };
        _repository.AddTaskCollaborator(collabTask, taskCollaborator);

    }



    private void ValidateCanManageCollaborators(CollaborativeTask selectedTask, CurrentUserDto currentUserDto)
    {
        var isAdmin = currentUserDto.CurrentUserSystemRole == SystemRole.Admin;
        var isOwner = selectedTask.UserId == currentUserDto.CurrentUserId;
        var isTaskAdmin = selectedTask.TaskCollaborators.Any(tc =>
            tc.UserId == currentUserDto.CurrentUserId &&
            tc.CollaboratorRole == CollaboratorRole.TaskAdministrator);

        if (!isAdmin && !isOwner && !isTaskAdmin)
            throw new UnauthorizedAccessException("No tienes permisos para gestionar colaboradores en la tarea.");
    }

    public void RemoveTaskCollaborator(int taskId, int userId, CurrentUserDto currentUserDto)
    {
        var selectedUser = _userRepository.GetUserById(userId) ?? throw new KeyNotFoundException($"No existe ningun usuario con el ID: {userId}");

        var selectedTask = _repository.GetTaskById(taskId) ?? throw new KeyNotFoundException($"No existe ninguna Tarea con el ID: {taskId}");

        if (selectedTask is not CollaborativeTask colTask)
            throw new ArgumentException($"La tarea seleccionada es del tipo({selectedTask.GetType().Name}) no es del tipo colaborativo.");

        ValidateCanManageCollaborators(colTask, currentUserDto);

        var taskCollaborator = colTask.TaskCollaborators.FirstOrDefault(tc => tc.UserId == userId) ?? throw new KeyNotFoundException($"El usuario con ID({userId}) NO está en el equipo.");

        _repository.RemoveTaskCollaborator(colTask, taskCollaborator);

    }

    public void CompleteTask(int taskId, CurrentUserDto currentUserDto)
    {
        var taskToComplete = _repository.GetTaskByIdWithRelations(taskId) ?? throw new KeyNotFoundException($"No existe ninguna tarea con el ID: {{taskId}}");

        ValidateCanEditTask(taskToComplete, currentUserDto);



        if (taskToComplete is CompositeTask composite && !composite.CanBeCompleted())
            throw new InvalidOperationException("No se puede completar una tarea compuesta con subtareas pendientes.");

        if (taskToComplete.Dependencies.Any(d => d.DependsOnTask.TaskStatus != TaskStatus.Completed))
            throw new InvalidOperationException("No se puede completar la tarea porque tiene dependencias pendientes.");

        taskToComplete.CompleteTask();
        _repository.UpdateTask(taskToComplete);
    }

    private void ValidateCanEditTask(Task taskToEdit, CurrentUserDto currentUserDto)
    {
        var userActive = EnsureActiveUser(currentUserDto);

        if (userActive.IsAdmin == true || currentUserDto.CurrentUserSystemRole == SystemRole.Admin) return;


        if (taskToEdit.UserId == currentUserDto.CurrentUserId) return;

        var userIsCollaborator = false;
        if (taskToEdit is CollaborativeTask collabTask)
        {
            userIsCollaborator = collabTask.TaskCollaborators?.Any(u => u.UserId == currentUserDto.CurrentUserId) ?? false;
        }

        if (userIsCollaborator) return;

        throw new UnauthorizedAccessException("No está autorizado para realizar esta operación");
    }

    public List<ResponseTaskDto>? GetLinkableTaskById(int taskId, CurrentUserDto currentUserDto)
    {
        var userActive = EnsureActiveUser(currentUserDto);
        var selectedTask = _repository.GetTaskById(taskId) ?? throw new KeyNotFoundException($"No existe ninguna Tarea con el ID: {taskId}");

        ValidateCanEditTask(selectedTask, currentUserDto);

        var list = _repository.GetAllTaskLinked(taskId) ?? new List<Models.Task>();
        var listDtos = new List<ResponseTaskDto>();

        foreach (var t in list)
        {
            listDtos.Add(new ResponseTaskDto
            {
                Id = t.Id,
                Title = t.Title,
                UserId = t.UserId,
                TaskDescription = t.TaskDescription,
                TaskType = t.TaskType,
                TaskPriority = t.TaskPriority,
                TaskStatus = t.TaskStatus,
                DueTime = t.DueTime,
                CancelReason = t.CancelReason,
            });
        }
        return listDtos;

    }

    public void DeleteLinkedRelation(int taskId, int linkedTaskId, CurrentUserDto currentUserDto)
    {
        var taskTarget = _repository.GetTaskById(taskId) ?? throw new KeyNotFoundException($"No existe ninguna Tarea con el ID: {taskId}");
        ValidateCanEditTask(taskTarget, currentUserDto);

        _repository.DeleteLinkedRelation(taskId, linkedTaskId);

    }

    private User EnsureActiveUser(CurrentUserDto currentUserDto)
    {
        var user = _userRepository.GetUserById(currentUserDto.CurrentUserId)
            ?? throw new KeyNotFoundException($"No existe ningún usuario con el ID: {currentUserDto.CurrentUserId}.");

        if (user.IsActive != true)
            throw new UnauthorizedAccessException("Usuario inactivo.");

        return user;
    }
    private static void ValidateDueTime(DateTime? dueTime)
    {
        if (!dueTime.HasValue || dueTime.Value == default) throw new ArgumentException("La fecha de vencimiento es obligatoria.");

        if (dueTime.Value <= DateTime.UtcNow) throw new ArgumentException("La fecha de vencimiento debe ser futura.");

        if (dueTime.Value > DateTime.UtcNow.AddYears(2)) throw new ArgumentException("La fecha de vencimiento no debe ser mayor a 2 años.");
    }
    public TasksRelationsDto GetTaskRelations(int taskId, CurrentUserDto currentUserDto)
    {
        var task = _repository.GetTaskByIdWithRelations(taskId) ?? throw new KeyNotFoundException($"No existe ninguna tarea con el ID: {taskId}.");

        ValidateCanEditTask(task, currentUserDto);

        var dto = new TasksRelationsDto
        {
            TaskId = task.Id,
            TaskType = task.TaskType
        };

        if (task is CompositeTask)
        {
            dto.SubTasks = _repository.GetSubTasksByCompositeTaskId(task.Id)
                .Select(st => (ResponseSubTaskDto)DtoManager.TaskToDto(st))
                .ToList();
        }

        if (task is RecurringTask recurring)
        {
            dto.RecurringIterations = _repository.GetRecurringTasksBySeriesId(recurring.RecurringSeriesId)
                .Select(rt => (ResponseRecurringTaskDto)DtoManager.TaskToDto(rt))
                .ToList();
        }

        dto.LinkedRelations = _repository.GetLinkedRelationsByTaskId(task.Id)
            .Select(ToLinkedRelationDto)
            .ToList();

        return dto;
    }
    public List<ResponseSubTaskDto> GetSubTasks(int compositeTaskId, CurrentUserDto currentUserDto)
    {
        var compositeTask = _repository.GetCompositeTaskById(compositeTaskId) ?? throw new KeyNotFoundException($"No existe ninguna tarea compuesta con el ID: {compositeTaskId}.");

        ValidateCanEditTask(compositeTask, currentUserDto);

        return _repository.GetSubTasksByCompositeTaskId(compositeTaskId)
            .Select(st => (ResponseSubTaskDto)DtoManager.TaskToDto(st))
            .ToList();
    }
    public List<ResponseRecurringTaskDto> GetRecurringIterations(int recurringTaskId, CurrentUserDto currentUserDto)
    {
        var recurringTask = _repository.GetRecurringTaskById(recurringTaskId) ?? throw new KeyNotFoundException($"No existe ninguna tarea recurrente con el ID: {recurringTaskId}.");

        ValidateCanEditTask(recurringTask, currentUserDto);

        return _repository.GetRecurringTasksBySeriesId(recurringTask.RecurringSeriesId)
            .Select(rt => (ResponseRecurringTaskDto)DtoManager.TaskToDto(rt))
            .ToList();
    }
    public List<ResponseLinkedTaskDto> GetLinkedRelations(int taskId, CurrentUserDto currentUserDto)
    {
        var task = _repository.GetTaskByIdWithRelations(taskId) ?? throw new KeyNotFoundException($"No existe ninguna tarea con el ID: {taskId}.");

        ValidateCanEditTask(task, currentUserDto);

        return _repository.GetLinkedRelationsByTaskId(taskId)
            .Select(ToLinkedRelationDto)
            .ToList();
    }
    private static ResponseLinkedTaskDto ToLinkedRelationDto(LinkedTask relation)
    {
        return new ResponseLinkedTaskDto
        {
            Id = relation.Id,
            TaskId = relation.TaskId,
            DependsOnTaskId = relation.DependsOnTaskId,
            LinkedTaskOrder = relation.LinkedTaskOrder ?? 0,
            Task = relation.Task is null ? null : ToResponseTaskDto(relation.Task),
            DependsOnTask = relation.DependsOnTask is null ? null : ToResponseTaskDto(relation.DependsOnTask)
        };
    }

    private static ResponseTaskDto ToResponseTaskDto(Task task)
    {
        return new ResponseTaskDto
        {
            Id = task.Id,
            Title = task.Title,
            UserId = task.UserId,
            TaskDescription = task.TaskDescription ?? string.Empty,
            TaskType = task.TaskType,
            TaskPriority = task.TaskPriority,
            TaskStatus = task.TaskStatus,
            DueTime = task.DueTime,
            CancelReason = task.CancelReason
        };
    }
}
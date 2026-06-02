using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;
using Task = GestorTareas.Models.Task;

namespace GestorTareas.Application.DTOs;

public static class DtoManager
{

    public static TaskDTO TaskToDto(Task task)
    {
        return task switch
        {
            SubTask sub => new ResponseSubTaskDto
            {
                Id = sub.Id,
                Title = sub.Title,
                UserId = sub.UserId,
                TaskDescription = sub.TaskDescription,
                TaskType = sub.TaskType,
                TaskPriority = sub.TaskPriority,
                TaskStatus = (int)sub.TaskStatus,
                DueTime = sub.DueTime,
                CancelReason = sub.CancelReason,
                ParentCompositeTaskId = sub.ParentCompositeTaskId
            },
            CompositeTask ct => new ResponseCompositeTaskDto
            {
                Id = ct.Id,
                Title = ct.Title,
                UserId = ct.UserId,
                TaskDescription = ct.TaskDescription,
                TaskType = ct.TaskType,
                TaskPriority = ct.TaskPriority,
                TaskStatus = (int?)ct.TaskStatus,
                DueTime = ct.DueTime,
                SubTasksList = ct.SubTaskList
                .Select(st => (ResponseSubTaskDto)TaskToDto(st))
                .ToList()
            },
            CollaborativeTask colt => new ResponseCollaborativeTaskDto
            {
                Id = colt.Id,
                Title = colt.Title,
                UserId = colt.UserId,
                TaskDescription = colt.TaskDescription,
                TaskType = colt.TaskType,
                TaskPriority = colt.TaskPriority,
                TaskStatus = (int?)colt.TaskStatus,
                DueTime = colt.DueTime,
                TaskCollaborators = colt.TaskCollaborators.Select(ConvertToTaskCollaboratorDto).ToList()
            },
            RecurringTask rt => new ResponseRecurringTaskDto
            {
                Id = rt.Id,
                Title = rt.Title,
                UserId = rt.UserId,
                DueTime = rt.DueTime,
                RecurrenceRule = rt.RecurrenceRule,
                RecurringTasksCount = rt.RecurringTasksCount,
                RecurringSeriesId = rt.RecurringSeriesId,
                TaskDescription = rt.TaskDescription,
                TaskType = rt.TaskType,
                TaskPriority = rt.TaskPriority,
                TaskStatus = (int)rt.TaskStatus,
                CancelReason = rt.CancelReason
            },

            SimpleTask st => new SimpleTaskDTO
            {
                Id = st.Id,
                Title = st.Title,
                UserId = st.UserId,
                TaskDescription = st.TaskDescription,
                TaskType = st.TaskType,
                TaskPriority = st.TaskPriority,
                TaskStatus = (int)st.TaskStatus,
                DueTime = st.DueTime,
                CancelReason = st.CancelReason
            },

            _ => throw new NotSupportedException("Tipo de tarea no soportado")
        };
    }

    private static TaskCollaboratorDto ConvertToTaskCollaboratorDto(TaskCollaborator collaborator)
    {
        return new TaskCollaboratorDto
        {
            UserId = collaborator.UserId,
            TaskId = collaborator.TaskId,
            CollaboratorRole = collaborator.CollaboratorRole,
        };
    }

    public static Task DtoToTask(TaskDTO taskDto)
    {
        return taskDto switch
        {
            ResponseSubTaskDto sub => new SubTask(
                sub.Title!,
                sub.UserId,
                sub.ParentCompositeTaskId,
                sub.TaskDescription!,
                sub.TaskType,
                (Priority)sub.TaskPriority,
                (TaskStatus)sub.TaskStatus,
                sub.DueTime,
                sub.CancelReason
            )
            { },
            ResponseCompositeTaskDto ct =>
                new CompositeTask(
                    ct.Title!,
                    ct.UserId,
                    ct.TaskDescription!,
                    ct.TaskType,
                    (Priority)ct.TaskPriority,
                    (Enums.TaskStatus)ct.TaskStatus,
                    ct.DueTime,
                    ct.CancelReason
                )
                {
                    Id = ct.Id,
                    SubTaskList = ct.SubTasksList.Select(st => new SubTask
                    {
                        Title = st.Title,
                        UserId = st.UserId,
                        TaskDescription = st.TaskDescription,
                        TaskType = st.TaskType,
                        TaskPriority = st.TaskPriority ?? Priority.Normal,
                        DueTime = st.DueTime,
                        ParentCompositeTaskId = st.ParentCompositeTaskId,
                    }).ToList(),
                },

            ResponseRecurringTaskDto rt => new RecurringTask(
                rt.Title!,
                rt.UserId,
                rt.DueTime,
                rt.RecurrenceRule,
                rt.RecurringTasksCount,
                rt.RecurringSeriesId,
                rt.TaskDescription!,
                rt.TaskType,
                (Priority)rt.TaskPriority,
                (Enums.TaskStatus)rt.TaskStatus,
                rt.CancelReason
            )
            {
                Id = rt.Id,
                RecurringTasksCount = rt.RecurringTasksCount
            },

            SimpleTaskDTO st => new SimpleTask(
                st.Title!,
                st.UserId,
                st.TaskDescription!,
                st.TaskType,
                (Enums.Priority)st.TaskPriority,
                (Enums.TaskStatus)st.TaskStatus,
                st.DueTime,
                st.CancelReason
            )
            {
                Id = st.Id
            },

            _ => throw new NotSupportedException("Tipo de DTO no soportado")
        };
    }

    private static List<TaskCollaboratorDto>? ConvertTaskCollaboratorsToDto(List<TaskCollaborator>? collaborators)
    {
        return collaborators?.Select(collaborator => new TaskCollaboratorDto
        {
            UserId = collaborator.UserId,
            TaskId = collaborator.TaskId,
            CollaboratorRole = collaborator.CollaboratorRole,
        }).ToList() ?? new List<TaskCollaboratorDto>(10);
    }


}

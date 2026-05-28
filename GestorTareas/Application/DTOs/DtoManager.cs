using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;
//using static GestorTareas.Models.Task;
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
                Priority = sub.Priority,
                Status = (int)sub.Status,
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
                Priority = ct.Priority,
                Status = (int?)ct.Status,
                DueTime = ct.DueTime,
                // SubTasksList = ct.SubTaskList,
            },
            CollaborativeTask colt => new ResponseCollaborativeTaskDto
            {
                Id = colt.Id,
                Title = colt.Title,
                UserId = colt.UserId,
                TaskDescription = colt.TaskDescription,
                TaskType = colt.TaskType,
                Priority = colt.Priority,
                Status = (int?)colt.Status,
                DueTime = colt.DueTime,
                TaskCollaborators = ConvertTaskCollaboratorsToDto(colt.TaskCollaborators)
            },
            RecurringTask rt => new ResponseRecurringTaskDto
            {
                Id = rt.Id,
                Title = rt.Title,
                UserId = rt.UserId,
                DueTime = rt.DueTime,
                RecurrenceRule = rt.RecurrenceRule,
                RecurringTasksCount = rt.RecurringTasksCount,
                TaskDescription = rt.TaskDescription,
                TaskType = rt.TaskType,
                Priority = rt.Priority,
                Status = (int)rt.Status,
                CancelReason = rt.CancelReason
            },

            SimpleTask st => new SimpleTaskDTO
            {
                Id = st.Id,
                Title = st.Title,
                UserId = st.UserId,
                TaskDescription = st.TaskDescription,
                TaskType = st.TaskType,
                Priority = st.Priority,
                Status = (int)st.Status,
                DueTime = st.DueTime,
                CancelReason = st.CancelReason
            },

            _ => throw new NotSupportedException("Tipo de tarea no soportado")
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
                (TaskPriority)sub.Priority,
                (TaskStatus)sub.Status,
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
                    (TaskPriority)ct.Priority,
                    (Enums.TaskStatus)ct.Status,
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
                        Priority = st.Priority ?? TaskPriority.Normal,
                        DueTime = st.DueTime,
                        ParentCompositeTaskId = st.ParentCompositeTaskId,
                    }).ToList(),
                },

            ResponseRecurringTaskDto rt => new RecurringTask(
                rt.Title!,
                rt.UserId,
                rt.DueTime,
                rt.RecurrenceRule,
                rt.TaskDescription!,
                rt.TaskType,
                (TaskPriority)rt.Priority,
                (Enums.TaskStatus)rt.Status,
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
                (TaskPriority)st.Priority,
                (Enums.TaskStatus)st.Status,
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

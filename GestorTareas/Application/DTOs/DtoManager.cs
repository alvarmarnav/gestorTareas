using System;
using System.Data.Common;
using System.Linq;
using GestorTareas.Enums;
using GestorTareas.Models;
using static GestorTareas.Models.Task;
using Task = GestorTareas.Models.Task;

namespace GestorTareas.Application.DTOs;

public static class DtoManager
{

    public static TaskDTO TaskToDto(Task task)
    {
        return task switch
        {
            SubTask sub => new SubTaskDTO
            {
                Id = sub.Id,
                Title = sub.Title,
                CompositeTaskType = (int)sub.CompositeTaskType,
                Description = sub.Description,
                Priority = (int)sub.Priority,
                Status = (int)sub.Status,
                DueTime = (DateTime)sub.DueTime,
                CancelReason = sub.CancelReason
            },
            LinkedTask link => new LinkedTaskDTO
            {
                Id = link.Id,
                Title = link.Title,
                CompositeTaskType = (int)link.CompositeTaskType,
                Description = link.Description,
                Priority = (int)link.Priority,
                Status = (int)link.Status,
                DueTime = (DateTime)link.DueTime,
                LinkedTaskOrder = (int)link.Order,
                CancelReason = link.CancelReason
            },

            CompositeTask ct => new CompositeTaskDTO
            {
                Id = ct.Id,
                Title = ct.Title,
                CompositeTaskType = (int)ct.CompositeTaskType,
                Description = ct.Description,
                Priority = (int)ct.Priority,
                Status = (int)ct.Status,
                DueTime = (DateTime)ct.DueTime,
                SubTasks = ct.SubTaskList,
                LinkedTaskList = ct.LinkedTaskList
            },

            RecurringTask rt => new RecurringTaskDTO
            {
                Id = rt.Id,
                Title = rt.Title,
                DueTime = (DateTime)rt.DueTime,
                RecurrenceRule = rt.RecurrenceRule,
                RecurringTasksCount = rt.RecurringTasksCount,
                Description = rt.Description,
                Priority = (int)rt.Priority,
                Status = (int)rt.Status,
                CancelReason = rt.CancelReason                
            },

            SimpleTask st => new SimpleTaskDTO
            {
                Id = st.Id,
                Title = st.Title,
                Description = st.Description,
                Priority = (int)st.Priority,
                Status = (int)st.Status,
                DueTime = (DateTime)st.DueTime,
                CancelReason = st.CancelReason
            },

            _ => throw new NotSupportedException("Tipo de tarea no soportado")
        };
    }

    public static Task DtoToTask(TaskDTO taskDto)
    {
       return taskDto switch
        {
            SubTaskDTO sub => new SubTask(
                sub.Title!,
                (Enums.CompositeTaskType)sub.CompositeTaskType,
                sub.Description!,
                (TaskPriority)sub.Priority,
                (Enums.TaskStatus)sub.Status,
                sub.DueTime,
                sub.CancelReason
            )
            {
                Id = sub.Id
            },
            LinkedTaskDTO link => new LinkedTask(
                link.Title!,
                (Enums.CompositeTaskType)link.CompositeTaskType,
                link.Description!,
                (TaskPriority)link.Priority,
                (Enums.TaskStatus)link.Status,
                link.DueTime,
                link.LinkedTaskOrder,
                link.CancelReason
            )
            {
                Id = link.Id
            },

            CompositeTaskDTO ct =>
                new CompositeTask(
                    ct.Title!,
                    (Enums.CompositeTaskType)ct.CompositeTaskType,
                    ct.Description!,
                    (TaskPriority)ct.Priority,
                    (Enums.TaskStatus)ct.Status,
                    ct.DueTime,
                    ct.CancelReason                    
                )
                {
                    Id = ct.Id,
                    SubTaskList = ct.SubTasks,
                    LinkedTaskList = ct.LinkedTaskList
                },

            RecurringTaskDTO rt => new RecurringTask(
                rt.Title!,
                rt.DueTime,
                rt.RecurrenceRule,
                rt.RecurringTasksCount,
                rt.Description!,
                (TaskPriority)rt.Priority,
                (Enums.TaskStatus)rt.Status,
                rt.CancelReason
            )
            {
                Id = rt.Id
            },

            SimpleTaskDTO st => new SimpleTask(
                st.Title!,
                st.Description!,
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

}

using System;
using System.Data.Common;
using System.Linq;
using GestorTareas.Enums;
using GestorTareas.Models;
//using static GestorTareas.Models.Task;
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
                UserId = sub.UserId,
                TaskDescription = sub.TaskDescription,
                Priority = (int)sub.Priority,
                Status = (int)sub.Status,
                DueTime = (DateTime)sub.DueTime,
                CancelReason = sub.CancelReason
            },
            CompositeTask ct => new CompositeTaskDTO
            {
                Id = ct.Id,
                Title = ct.Title,
                UserId = ct.UserId,
                TaskDescription = ct.TaskDescription,
                Priority = (int)ct.Priority,
                Status = (int)ct.Status,
                DueTime = (DateTime)ct.DueTime,
                SubTasks = ct.SubTaskList,
            },

            RecurringTask rt => new RecurringTaskDTO
            {
                Id = rt.Id,
                Title = rt.Title,
                UserId = rt.UserId,
                DueTime = (DateTime)rt.DueTime,
                RecurrenceRule = rt.RecurrenceRule,
                RecurringTasksCount = rt.RecurringTasksCount,
                TaskDescription = rt.TaskDescription,
                Priority = (int)rt.Priority,
                Status = (int)rt.Status,
                CancelReason = rt.CancelReason
            },

            SimpleTask st => new SimpleTaskDTO
            {
                Id = st.Id,
                Title = st.Title,
                UserId = st.UserId,
                TaskDescription = st.TaskDescription,
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
                sub.UserId,
                sub.TaskDescription!,
                (TaskPriority)sub.Priority,
                (Enums.TaskStatus)sub.Status,
                sub.DueTime,
                sub.CancelReason
            )
            {
                Id = sub.Id,
            },
            CompositeTaskDTO ct =>
                new CompositeTask(
                    ct.Title!,
                    ct.UserId,
                    ct.TaskDescription!,
                    (TaskPriority)ct.Priority,
                    (Enums.TaskStatus)ct.Status,
                    ct.DueTime,
                    ct.CancelReason
                )
                {
                    Id = ct.Id,
                    SubTaskList = ct.SubTasks,
                },

            RecurringTaskDTO rt => new RecurringTask(
                rt.Title!,
                rt.UserId,
                rt.DueTime,
                rt.RecurrenceRule,

                rt.TaskDescription!,
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

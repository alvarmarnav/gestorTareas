using System;
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
                Priority = (int)sub.Priority,
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
                Priority = ct.Priority,
                Status = (int?)ct.Status,
                DueTime = ct.DueTime,
                // SubTasksList = ct.SubTaskList,
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
                (TaskPriority)sub.Priority,
                (TaskStatus)sub.Status,
                sub.DueTime,
                sub.CancelReason
            )
            {},
            CreateCompositeTaskDto ct =>
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
                    SubTaskList = ct.SubTaskList,
                },

            ResponseRecurringTaskDto rt => new RecurringTask(
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

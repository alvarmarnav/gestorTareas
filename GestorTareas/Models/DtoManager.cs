using System;
using System.Data.Common;
using System.Linq;
using static GestorTareas.Models.Task;

namespace GestorTareas.Models;

public static class DTOManager
{

    public static TaskDTO TaskToDto(Task task)
    {
        return task switch
        {
            SubTask sub => new SubTaskDTO
            {
                Id = sub.Id,
                Title = sub.Title,
                Description = sub.Description,
                Priority = (int)sub.Priority,
                Status = (int)sub.Status,
                DueTime = (DateTime)sub.DueTime,
                // ParentId = sub.ParentId
            },

            CompositeTask ct => new CompositeTaskDTO
            {
                Id = ct.Id,
                Title = ct.Title,
                Description = ct.Description,
                Priority = (int)ct.Priority,
                Status = (int)ct.Status,
                DueTime = (DateTime)ct.DueTime,
                // SubTasks = ct.SubTasks?.Select(TaskToDto).ToList()
            },

            RecurringTask rt => new RecurringTaskDTO
            {
                Id = rt.Id,
                Title = rt.Title,
                Description = rt.Description,
                Priority = (int)rt.Priority,
                Status = (int)rt.Status,
                DueTime = (DateTime)rt.DueTime,
                RecurrenceRule = rt.RecurrenceRule
            },

            SimpleTask st => new SimpleTaskDTO
            {
                Id = st.Id,
                Title = st.Title,
                Description = st.Description,
                Priority = (int)st.Priority,
                Status = (int)st.Status,
                DueTime = (DateTime)st.DueTime
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
                sub.Description!,
                (TaskPriority)sub.Priority,
                (Task.TaskStatus)sub.Status,
                sub.DueTime
                // sub.ParentId
            )
            {
                Id = sub.Id
            },

            CompositeTaskDTO ct =>
                new CompositeTask(
                    ct.Title!,
                    ct.Description!,
                    (TaskPriority)ct.Priority,
                    (Task.TaskStatus)ct.Status,
                    ct.DueTime
                )
                {
                    Id = ct.Id,
                //     SubTasks = ct.SubTasks?.Select(DtoToTask).ToList()
                },

            RecurringTaskDTO rt => new RecurringTask(
                rt.Title!,
                rt.Description!,
                (TaskPriority)rt.Priority,
                (Task.TaskStatus)rt.Status,
                rt.DueTime,
                rt.RecurrenceRule
            )
            {
                Id = rt.Id
            },

            SimpleTaskDTO st => new SimpleTask(
                st.Title!,
                st.Description!,
                (TaskPriority)st.Priority,
                (Task.TaskStatus)st.Status,
                st.DueTime
            )
            {
                Id = st.Id
            },

            _ => throw new NotSupportedException("Tipo de DTO no soportado")
        };
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using GestorTareas.Models;

namespace GestorTareas.Models;

public class DtoManager
{
    protected static readonly List<TaskDTO> _dtoList = new(60);

    public static void ConvertTaskToDto(Task item){
        ArgumentNullException.ThrowIfNull(item);

        TaskDTO dto = item switch
        {
            SimpleTask st => new SimpleTaskDTO()
            {
                Id=st.Id,
                Title = st.Title,
                Description = st.Description ?? string.Empty,
                Priority = (Task.TaskPriority) st.Priority,
                Status = (TaskStatus)st.Status,
                DueTime = st.DueTime ?? DateTime.Now.AddDays(7)
            },
            RecurringTask rt => new RecurringTaskDTO()
            {
                Id = rt.Id,
                Title = rt.Title,
                Description = rt.Description ?? string.Empty,
                Priority = (Task.TaskPriority)rt.Priority,
                Status = (TaskStatus)rt.Status,
                DueTime = rt.DueTime ??  DateTime.Now.AddDays(7),
                RecurrenceRule = (int)rt.RecurrenceRule,
            },
            SubTask subt => new SubTaskDTO()
            {
                Id = subt.Id,
                Title = subt.Title,
                Description = subt.Description,
                Priority = (Task.TaskPriority)subt.Priority,
                Status = (TaskStatus)subt.Status,
                DueTime = subt.DueTime ?? DateTime.Now.AddDays(7),
                SubTaskOrder = (int)subt.SubTaskOrder,
            },
            CompositeTask ct => new CompositeTaskDTO()
            {
                Id = ct.Id,
                Title = ct.Title,
                Description = ct.Description ?? string.Empty,
                Priority = (Task.TaskPriority) ct.Priority,
                Status = (TaskStatus)ct.Status,
                DueTime = ct.DueTime ?? DateTime.Now.AddDays(7),
            },
            LinkedTask lt => new LinkedTaskDTO(){
                Id = lt.Id,
                Title = lt.Title,
                Description = lt.Description ?? string.Empty,
                Priority = (Task.TaskPriority)lt.Priority,
                Status = (TaskStatus)lt.Status,
                DueTime = lt.DueTime ?? DateTime.Now.AddDays(7),
                LinkedTaskOrder = lt.LinkedTaskOrder ?? 0
            },
            _ => throw new ArgumentException("Error, no es válido el tipo de tarea")
        };

    }

    // public static TaskDTO ConvertToDto(IEnumerable<T> itemList)
    // {

    //     List<T>? dtoList = new List<T>(itemList.Count());

    //     foreach(var item in itemList)
    //     {
    //         var dto = item switch
    //         {
    //             SimpleTask st => new SimpleTaskDTO()
    //             {
    //                 Id=st.Id,
    //                 Title = st.Title,
    //                 Description = st.Description,
    //                 Priority = (Task.TaskPriority) st.Priority,
    //                 Status = (TaskStatus)st.Status,
    //                 DueTime = (DateTime)st.DueTime,
    //             },
    //             RecurringTask rt => new RecurringTaskDTO()
    //             {
    //                 Id = rt.Id,
    //                 Title = rt.Title,
    //                 Description = rt.Description,
    //                 Priority = (Task.TaskPriority)rt.Priority,
    //                 Status = (TaskStatus)rt.Status,
    //                 DueTime = (DateTime)rt.DueTime,
    //                 RecurrenceRule = (int)rt.RecurrenceRule
    //             },
    //             CompositeTask ct => new CompositeTaskDTO()
    //             {
    //                 Id = ct.Id,
    //                 Title = ct.Title,
    //                 Description = ct.Description,
    //                 Priority = (Task.TaskPriority) ct.Priority,
    //                 Status = (TaskStatus)ct.Status,
    //                 DueTime = (DateTime)ct.DueTime
    //             },
    //             SubTask st => new SubTaskDTO()
    //             {
    //                 Id = st.Id,
    //                 Title = st.Title,
    //                 Description = st.Description,
    //                 Priority = (Task.TaskPriority)st.Priority,
    //                 Status = (TaskStatus)st.Status,
    //                 DueTime = (DateTime)st.DueTime
    //             },
    //             _ => throw new ArgumentException("Error, no es válido el tipo de tarea")
    //         }; 
    //     }
    // }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace GestorTareas.Models;

public class LinkedTask : CompositeTask
{
    //TODO: pendiente linkedTask logica de dependencias.

    public List<LinkedTask> ListOfLinkedTasks { get; set; } = new(60);
    public int? Order{get;set;} = null;

    [JsonConstructor]
    public LinkedTask() : base() { }
    public LinkedTask(
        string title,
        string? description,
        TaskPriority? taskPriority = TaskPriority.Normal,
        TaskStatus? taskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        int? order = null
        ) : base(
            title,
            description,
            taskPriority,
            taskStatus,
            dueTime
            )
    {
        // null--> ultimo
        // menor 0 --> error 
        // igual 0 --> primero 
        // mayor del tamaño del List--> ultimo
        Order = order;
        ListOfLinkedTasks ??= new(60);

    }

    public void UpdateLinkedTaskOrder(int newOrder) { }

    public void CanStartLinkedTask(Guid linkedTaskId) { }

    public void CompleteLinkedTask(Guid linkedTaskId) {
        
        LinkedTask task = ListOfLinkedTasks.Find(lt => lt.Id == linkedTaskId);
        if(task is null)
            throw new ArgumentException("El identificador no es válido.");

        else if(task.Status == TaskStatus.Pending || task.Status == TaskStatus.InProgress)
        {
            task.Status = TaskStatus.Completed;
        }
    }

    public bool CanStartLinkedTask(LinkedTask lTask)
    {
        
        if (ListOfLinkedTasks is null || !ListOfLinkedTasks.Any())
            return false;

        else
        {
            foreach (var t in ListOfLinkedTasks.Where(t => t.Id != lTask.Id))
            {
                if (t.Status == TaskStatus.InProgress || t.Status == TaskStatus.Pending)
                {
                    return false;
                }

                lTask.Status = TaskStatus.InProgress;

            }
            return true;
        }
    }

    public override string ResumeTask()
    {
        throw new NotImplementedException();
    }

}

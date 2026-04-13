using System;
using GestorTareas.Interfaces;

namespace GestorTareas.Models;

public class SubTask : CompositeTask
{

    public int? SubTaskOrder { get; set; }

    public SubTask(string subTaskTitle, string subTaskDescription, TaskPriority subTaskPriority = TaskPriority.Normal, TaskStatus subTaskStatus = TaskStatus.Pending, int subTaskOrder = 0) : base(subTaskTitle, subTaskDescription, subTaskPriority, subTaskStatus)
    {
        this.SubTaskOrder = subTaskOrder;
    }


    public void CompleteSubTask()
    {

    }

    public void ReopenSubTask()
    {

    }
    public void UpdateSubTaskDescription(string newDescription)
    {

    }
    public void UpdateSubTaskTitle(string newTitle) { }

    public void UpdateSubTaskOrder(int newOrder) { }



}

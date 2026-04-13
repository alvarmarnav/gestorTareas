using System;
using GestorTareas.Interfaces;

namespace GestorTareas.Models;

public class SubTask : CompositeTask
{

    public int? SubTaskOrder { get; set; }

    public SubTask(
        string subTaskTitle,
        string subTaskDescription,
        TaskPriority subTaskPriority,
        TaskStatus subTaskStatus,
        DateTime dueTime,
        int? subTaskOrder =null) : base(
            subTaskTitle,
            subTaskDescription,
            subTaskPriority = TaskPriority.Normal,
            subTaskStatus=TaskStatus.Pending,
            dueTime)
    {
        this.SubTaskOrder = --subTaskOrder;
    }


    // public void CompleteSubTask()
    // {

    // }

    // public void ReopenSubTask()
    // {

    // }
    // public void UpdateSubTaskDescription(string newDescription)
    // {

    // }
    // public void UpdateSubTaskTitle(string newTitle) { }

    public void UpdateSubTaskOrder(int newOrder)
    {
        //DEBO ACCEDER A LA LIST SUBTASK DEL ELEMENTO SUPERIOR
        //PARA RESTRINGIR QUE NO PUEDA AÑADIR UN PUESTO
        //FUERA DE RANGO
    }

    public override string ResumeTask()
    {
        return base.ResumeTask();
    }
}

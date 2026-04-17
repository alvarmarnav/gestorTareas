using System;
using System.Text.Json.Serialization;
using GestorTareas.Interfaces;

namespace GestorTareas.Models;

public class SubTask : CompositeTask
{

    public int? SubTaskOrder { get; set; }

// ESTO ES LO QUE FALTA:
    [JsonConstructor]
    public SubTask() : base() { } 
    public SubTask(
        string subTaskTitle,
        string? subTaskDescription = null,
        TaskPriority? subTaskPriority = TaskPriority.Normal,
        TaskStatus? subTaskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        int? subTaskOrder = null) : base(
            subTaskTitle,
            subTaskDescription,
            subTaskPriority,
            subTaskStatus,
            dueTime)
    {
        if(_subTaskList.Count>0)
            this.SubTaskOrder = --subTaskOrder;
        else
            this.SubTaskOrder = 0;
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
        return $"SubTarea Id: {this.Id}\nTitulo: {Title}\nDescripción: {this.Description}\nPrioridad: {this.Priority}\nEstado: {this.Status}";

    }
}

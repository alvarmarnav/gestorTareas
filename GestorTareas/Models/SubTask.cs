using System;
using System.Text.Json.Serialization;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using GestorTareas.Interfaces;
using TaskStatus = GestorTareas.Enums.TaskStatus;
using TaskPriority = GestorTareas.Enums.TaskPriority;


namespace GestorTareas.Models;

public class SubTask : CompositeTask
{

    // ESTO ES LO QUE FALTA:
    [JsonConstructor]
    public SubTask() : base() { }
    public SubTask(
        string subTaskTitle,
        CompositeTaskType compositeTaskType,
        string? subTaskDescription = null,
        TaskPriority? subTaskPriority = TaskPriority.Normal,
        TaskStatus? subTaskStatus = TaskStatus.Pending,
        DateTime? dueTime = null,
        string? cancelReason = null
        ) : base(
            subTaskTitle,
            compositeTaskType,
            subTaskDescription,
            subTaskPriority,
            subTaskStatus,
            dueTime,
            cancelReason)
    {

    }

    public void UpdateSubTaskOrder(int newOrder)
    {
        //DEBO ACCEDER A LA LIST SUBTASK DEL ELEMENTO SUPERIOR
        //PARA RESTRINGIR QUE NO PUEDA AÑADIR UN PUESTO
        //FUERA DE RANGO
    }

    public override string ResumeTask() =>  $"SubTarea Id: {Id}\nTitulo: {Title}\nDescripción: {Description}\nPrioridad: {Priority}\nEstado: {Status}";
   
}

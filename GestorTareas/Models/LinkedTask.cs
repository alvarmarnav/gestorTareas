using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using GestorTareas.Application.Services;
using GestorTareas.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Models;

public class LinkedTask
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public Task Task { get; set; }
    public int? DependsOnTaskId { get; set; }=null;
    public Task DependsOn { get; set; }
    public int? LinkedTaskOrder { get; set; }=null;


    public LinkedTask() { }

    public LinkedTask(int taskId, int dependsOnTaskId, int linkedTaskOrder)
    {
        if (taskId <= 0)
            throw new ArgumentException("Id de la tarea no válido.");
        if (dependsOnTaskId <= 0)
            throw new ArgumentException("Id de la tarea de la que depende no válido.");
        if (linkedTaskOrder <= 0)
            throw new ArgumentException("Posición de la tarea no válida.");
        TaskId=taskId;
        DependsOnTaskId=dependsOnTaskId;
        LinkedTaskOrder=linkedTaskOrder;

    }
    // public void UpdateLinkedTaskOrder(int newOrder) { }
    public void CompleteLinkedTask(int linkedTaskId)
    {
        if (linkedTaskId <= 0 || linkedTaskId != Id)
            throw new ArgumentException("El identificador no es válido o bien No existe la tarea.");
        if (Task.Status == TaskStatus.Completed)
            throw new ArgumentException("Tarea YA Completada anteriormente.");
        if (DependsOn.Status != TaskStatus.Completed)
            throw new InvalidOperationException($"Existen tareas previas SIN Completar.");

        Task.Status = TaskStatus.Completed;
    }

    public bool CanStartLinkedTask(LinkedTask lTask)
    {
        if (lTask.DependsOn is null || lTask.DependsOn.Status != TaskStatus.Completed)
            return false;
        else
            return true;
    }

    // public override string ResumeTask() => $"Tarea Enlazada Id: {Id}\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {Priority}\nEstado: {Status}\nFecha Limite: {DueTime}\nOrden: {LinkedTaskOrder}";

}

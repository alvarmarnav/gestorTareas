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
    public int DependsOnTaskId { get; set; }
    public Task DependsOn { get; set; }
    public int LinkedTaskOrder { get; set; }

    
    // public void UpdateLinkedTaskOrder(int newOrder) { }
    public void CompleteLinkedTask(int linkedTaskId)
    {
        if (linkedTaskId<=0 && linkedTaskId != TaskId)
            throw new ArgumentException("El identificador no es válido o bien No existe la tarea.");
        if (Task.Status == TaskStatus.Completed)
            throw new ArgumentException("Tarea YA Completada anteriormente.");
        if (DependsOn.Status != TaskStatus.Completed)
            throw new InvalidOperationException($"Existen tareas previas SIN Completar.");      

        Task.Status = TaskStatus.Completed;
    }

    public bool CanStartLinkedTask(LinkedTask lTask)
    {
        if (DependsOn is null || DependsOn.Status != TaskStatus.Completed)
            return false;
        else    
            return true;
    }

    // public override string ResumeTask() => $"Tarea Enlazada Id: {Id}\nTitulo: {Title}\nDescripción: {TaskDescription}\nPrioridad: {Priority}\nEstado: {Status}\nFecha Limite: {DueTime}\nOrden: {LinkedTaskOrder}";

}

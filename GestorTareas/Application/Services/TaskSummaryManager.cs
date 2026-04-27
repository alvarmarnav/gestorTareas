using System;

namespace GestorTareas.Application.Services;

using GestorTareas.Models;

public class TaskSummaryManager
{

    public class TaskSummaryManager() { }
    public string ResumeTask(Task itemTask)
    {
        var taskType = (string)itemTask.GetType().Name;

        switch (taskType)
        {
            case "SimpleTask":

                return $"Tarea Simple\nTitulo: {itemTask.Title}\nDescripción: {itemTask.Description}\nPrioridad: {itemTask.Priority}\nEstado: {itemTask.Status}";

            case "CollaborativeTask":
                return $"Tarea Colaborativa\nTitulo: {itemTask.Title}\nDescripción: {itemTask.Description}\nPrioridad: {itemTask.Priority}\nEstado: {itemTask.Status}";


            case "CompositeTask":
                return $"Tarea con Subtareas\nTitulo: {itemTask.Title}\nDescripción: {itemTask.Description}\nPrioridad: {itemTask.Priority}\nEstado: {itemTask.Status}\nFecha Limite: {itemTask.DueTime}\nNumero Subtareas: {itemTask.SubTaskList.Count()}";

            case "LinkedTask":
                return $"Tarea Enlazada Id: {itemTask.Id}\nTitulo: {itemTask.Title}\nDescripción: {itemTask.Description}\nPrioridad: {itemTask.Priority}\nEstado: {itemTask.Status}\nFecha Limite: {itemTask.DueTime}\nOrden: {itemTask}";
                ;

            case "RecurringTask":
                return $"Tarea Recurrente\nTitulo: {itemTask.Title}\nDescripción: {itemTask.Description}\nPrioridad: {itemTask.Priority}\nEstado: {itemTask.Status}\nFecha Fin: {itemTask.DueTime}\nRegla Recurrencia: {itemTask.RecurrenceRule}";

            case "SubTask":
                return $"SubTarea Id: {itemTask.Id}\nTitulo: {itemTask.Title}\nDescripción: {itemTask.Description}\nPrioridad: {itemTask.Priority}\nEstado: {itemTask.Status}";

            default:
                return "Error no exite.";
        }
    }
}
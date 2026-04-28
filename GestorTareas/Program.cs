using System.Formats.Tar;
using System.Net;
using GestorTareas;
using GestorTareas.Application.Services;
using GestorTareas.Infraestructure.Repositories;
using CompositeTaskType = GestorTareas.Enums.CompositeTaskType;
using GestorTareas.Models;
using Microsoft.VisualBasic;
using Task = GestorTareas.Models.Task;


var repo = new TaskRepository();
var manager = new TaskManager(repo);

// ******************************
// ******CREACION DE TAREAS******
// ******************************
var taskList = new List<Task>
{
    new SimpleTask(
        "Revisar pull request",
        "Descripcion",
        GestorTareas.Enums.TaskPriority.Normal,
        GestorTareas.Enums.TaskStatus.Pending,
        DateTime.Today.AddDays(1),
        ""),
    // new RecurringTask(
    //     "Reunión de equipo",
    //     DateTime.Today.AddDays(25),
    //     7,
    //     "",
    //     GestorTareas.Enums.TaskPriority.Normal,
    //     GestorTareas.Enums.TaskStatus.Pending),
    // new LinkedTask(
    //            "Linked Task",
    //            GestorTareas.Enums.CompositeTaskType.LinkedTask,
    //            "",
    //           GestorTareas.Enums.TaskPriority.Critical,
    //         GestorTareas.Enums.TaskStatus.Completed,
    //             DateTime.Today.AddDays(25)),
    //         new SubTask(
    //             "SubTAsk",
    //             GestorTareas.Enums.CompositeTaskType.SubTask,
    //             "",
    //             GestorTareas.Enums.TaskPriority.High,
    //             GestorTareas.Enums.TaskStatus.Pending,
    //             DateTime.Today.AddDays(31)
    //         ),
    //         new RecurringTask(
    //         "Reunión de equipo",
    //         DateTime.Today.AddDays(25),
    //         7,
    //         "",
    //         GestorTareas.Enums.TaskPriority.Normal,
    //         GestorTareas.Enums.TaskStatus.Pending)
            };

//*************************
// Añadiendo las creadas
//*************************
foreach (var t in taskList)
{
    manager.AddTask(t);

}

manager.SaveRepository();

var lista = manager.ShowAllItems();

int cont = 0;
foreach (var item in lista)
{
    Console.WriteLine($"Tarea: {++cont}\n{item.ResumeTask()}");
    // item.ResumeTask();
}

cont = 0;
Guid selectedId = manager.ShowAllItems().First().Id;
Console.WriteLine($"El GUID: {selectedId}");





manager.RemoveTask(selectedId);



manager.SaveRepository();

Console.WriteLine($"Numero Tasks: {manager.ShowAllItems().Count()}");




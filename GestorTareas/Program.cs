using System.Formats.Tar;
using System.Net;
using GestorTareas;
using GestorTareas.Application.Services;
using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Models;
using Microsoft.VisualBasic;


var repo = new TaskRepository();
var manager = new TaskManager(repo);

// ******************************
// ******CREACION DE TAREAS******
// ******************************
var taskList = new List<GestorTareas.Models.Task>
{
    new SimpleTask(
        "Revisar pull request",
        "Descripcion",
        GestorTareas.Enums.TaskPriority.Normal,
        GestorTareas.Enums.TaskStatus.Pending,
        DateTime.Today.AddDays(1)),
    new RecurringTask(
        "Reunión de equipo",
            "",
            GestorTareas.Enums.TaskPriority.Normal,
            GestorTareas.Enums.TaskStatus.Pending,
            DateTime.Today.AddDays(7),
            recurrenceRule:7),
            new CompositeTask(
               "Recurring Task",
               "",
               GestorTareas.Enums.TaskPriority.Critical,
               GestorTareas.Enums.TaskStatus.Completed,
               DateTime.Today.AddDays(25)
            ),
            new SubTask(
                "SubTAsk",
                "",
                GestorTareas.Enums.TaskPriority.High,
                GestorTareas.Enums.TaskStatus.Pending,
                DateTime.Today.AddDays(31)
            ),
            // CreateSubTask(
            //     "SubTAsk",
            //     "",
            //     GestorTareas.Enums.TaskPriority.High,
            //     GestorTareas.Enums.TaskStatus.Pending,
            //     DateTime.Today.AddDays(31),
            //     0),
                 new RecurringTask(
            "Reunión de equipo 2",
            DateTime.Today.AddDays(30),
            98,
            "",
            GestorTareas.Enums.TaskPriority.Normal,
            GestorTareas.Enums.TaskStatus.Pending
           ),
            // ),
            // new SubTask( "SubTAsk2",
            //     "",
            //     GestorTareas.Enums.TaskPriority.High,
            //     GestorTareas.Enums.TaskStatus.Pending,
            //     DateTime.Today.AddDays(31),
            //     0)
};

//*************************
// Añadiendo las creadas
//*************************
foreach(var t in taskList)
{
    manager.AddTask(t);

}
// Console.WriteLine(TaskManager._taskList);

//Serializar
// TaskSerializer<GestorTareas.Models.Task>.SerializateListTaskToJson(TaskManager._taskList);



// //Deserializar
// IEnumerable<GestorTareas.Models.Task> deserializedTasks = TaskSerializer<GestorTareas.Models.Task>.DeserializeJsonList();
// var cont = 0;

// foreach(var t in deserializedTasks)
// {

//     Console.WriteLine($"Nª: {++cont} __ "+t.ResumeTask()+$"\n");
// }

// SimpleTask simpleTask1 = new SimpleTask(
//     "Titulo simpleTask1",
//     "Descripcion simpleTask1",
//     GestorTareas.Enums.TaskPriority.Normal,
//     GestorTareas.Enums.TaskStatus.Pending,
//     DateTime.Today.AddDays(10)
// );

// TaskManager.AddTask(simpleTask1);



// var task = new SimpleTask(
//     title: "Viajar a Turin",
//     description: "Recorrer Turin",
//     taskStatus: GestorTareas.Enums.TaskStatus.InProgress,
//     taskPriority: GestorTareas.Enums.TaskPriority.High,
//     dueTime: DateTime.Now.AddHours(65)
// );
// manager.AddTask(task);

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



// var selectedItem = manager.IdSearch(manager.TaskDictionary
// .Where(t => t.Value.Id == selectedId)
// .Select(t=> t.Value.Id));

manager.RemoveTask(selectedId);

// manager.TaskList.RemoveAll(t =>
// {
//     return true;
// });

manager.SaveRepository();

Console.WriteLine($"Numero Tasks: {manager.ShowAllItems().Count()}");

// lista = manager.ShowAllItems();
// Console.WriteLine();
// if(lista.Count()<=0)
//     Console.WriteLine("NAda");

// foreach (var item in lista)
// {
//     Console.WriteLine($"Tarea: {++cont}\n{item.ResumeTask()}");
//     // item.ResumeTask();
// }


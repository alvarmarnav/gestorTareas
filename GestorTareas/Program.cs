using System.Formats.Tar;
using System.Net;
using GestorTareas;
using GestorTareas.Models;
using Microsoft.VisualBasic;


// //Creacion 1 tarea
// var compositeT =  new CompositeTask(
//                "Recurring Task",
//                "",
//                GestorTareas.Models.Task.TaskPriority.Critical,
//                GestorTareas.Models.Task.TaskStatus.Completed,
//                DateTime.Today.AddDays(25) 
//             );

// // //Creacion varias tareas
// var taskList = new List<GestorTareas.Models.Task>
// {
//     new SimpleTask(
//         "Revisar pull request",
//         "Descripcion",
//         GestorTareas.Models.Task.TaskPriority.Normal,
//         GestorTareas.Models.Task.TaskStatus.Pending,
//         DateTime.Today.AddDays(1)),
//         new RecurringTask(
//             "Reunión de equipo",
//             "",
//             GestorTareas.Models.Task.TaskPriority.Normal,
//             GestorTareas.Models.Task.TaskStatus.Pending,
//             DateTime.Today.AddDays(7),
//             recurrenceRule:7),
//             // new CompositeTask(
//             //    "Recurring Task",
//             //    "",
//             //    GestorTareas.Models.Task.TaskPriority.Critical,
//             //    GestorTareas.Models.Task.TaskStatus.Completed,
//             //    DateTime.Today.AddDays(25) 
//             // ),
//             // new SubTask(
//             //     "SubTAsk",
//             //     "",
//             //     GestorTareas.Models.Task.TaskPriority.High,
//             //     GestorTareas.Models.Task.TaskStatus.Pending,
//             //     DateTime.Today.AddDays(31)
//             // )
//             // compositeT,
//             compositeT.CreateSubTask(
//                 "SubTAsk",
//                 "",
//                 GestorTareas.Models.Task.TaskPriority.High,
//                 GestorTareas.Models.Task.TaskStatus.Pending,
//                 DateTime.Today.AddDays(31),
//                 0),
//                  new RecurringTask(
//             "Reunión de equipo 2",
//             "",
//             GestorTareas.Models.Task.TaskPriority.Normal,
//             GestorTareas.Models.Task.TaskStatus.Pending,
//             DateTime.Today.AddDays(30),
//             recurrenceRule:98),
//             // ),
//             // new SubTask( "SubTAsk2",
//             //     "",
//             //     GestorTareas.Models.Task.TaskPriority.High,
//             //     GestorTareas.Models.Task.TaskStatus.Pending,
//             //     DateTime.Today.AddDays(31),
//             //     0)
// };


//Añadiendo las creadas
// foreach(var t in taskList)
// {
//     TaskManager.AddTask(t);

// }
// Console.WriteLine(TaskManager._taskList);

//Serializar
// TaskSerializer<GestorTareas.Models.Task>.SerializateListTaskToJson(TaskManager._taskList);



// //Deserializar
// IEnumerable<GestorTareas.Models.Task> deserializedTasks = TaskSerializer<GestorTareas.Models.Task>.DesSerializeJsonList();
// var cont = 0;

// foreach(var t in deserializedTasks)
// {

//     Console.WriteLine($"Nª: {++cont} __ "+t.ResumeTask()+$"\n");
// }

// SimpleTask simpleTask1 = new SimpleTask(
//     "Titulo simpleTask1",
//     "Descripcion simpleTask1",
//     GestorTareas.Models.Task.TaskPriority.Normal,
//     GestorTareas.Models.Task.TaskStatus.Pending,
//     DateTime.Today.AddDays(10)
// );

// TaskManager.AddTask(simpleTask1);

var repo = new TaskRepository();
var manager = new TaskManager(repo);

// var task = new SimpleTask(
//     title: "Viajar a Turin",
//     description: "Recorrer Turin",
//     taskStatus: GestorTareas.Models.Task.TaskStatus.InProgress,
//     taskPriority: GestorTareas.Models.Task.TaskPriority.High,
//     dueTime: DateTime.Now.AddHours(15)
// );
// manager.AddTask(task);

// manager.SaveRepository();

var lista = manager.ShowAllItems();

int cont = 0;
foreach (var item in lista)
{
    Console.WriteLine($"Tarea: {++cont}\n{item.ResumeTask()}");
    // item.ResumeTask();
}

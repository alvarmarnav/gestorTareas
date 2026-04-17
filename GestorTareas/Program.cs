using System.Formats.Tar;
using GestorTareas;
using GestorTareas.Models;
using Microsoft.VisualBasic;

var compositeT =  new CompositeTask(
               "Recurring Task",
               "",
               GestorTareas.Models.Task.TaskPriority.Critical,
               GestorTareas.Models.Task.TaskStatus.Completed,
               DateTime.Today.AddDays(25) 
            );


var taskList = new List<GestorTareas.Models.Task>
{
    new SimpleTask(
        "Revisar pull request",
        "Descripcion",
        GestorTareas.Models.Task.TaskPriority.Normal,
        GestorTareas.Models.Task.TaskStatus.Pending,
        DateTime.Today.AddDays(1)),
        new RecurringTask(
            "Reunión de equipo",
            "",
            GestorTareas.Models.Task.TaskPriority.Normal,
            GestorTareas.Models.Task.TaskStatus.Pending,
            DateTime.Today.AddDays(7),
            recurrenceRule:7),
            // new CompositeTask(
            //    "Recurring Task",
            //    "",
            //    GestorTareas.Models.Task.TaskPriority.Critical,
            //    GestorTareas.Models.Task.TaskStatus.Completed,
            //    DateTime.Today.AddDays(25) 
            // ),
            // new SubTask(
            //     "SubTAsk",
            //     "",
            //     GestorTareas.Models.Task.TaskPriority.High,
            //     GestorTareas.Models.Task.TaskStatus.Pending,
            //     DateTime.Today.AddDays(31)
            // )
            compositeT,
            compositeT.CreateSubTask(
                "SubTAsk",
                "",
                GestorTareas.Models.Task.TaskPriority.High,
                GestorTareas.Models.Task.TaskStatus.Pending,
                DateTime.Today.AddDays(31),
                0),
                 new RecurringTask(
            "Reunión de equipo 2",
            "",
            GestorTareas.Models.Task.TaskPriority.Normal,
            GestorTareas.Models.Task.TaskStatus.Pending,
            DateTime.Today.AddDays(30),
            recurrenceRule:98),
            // ),
            // new SubTask( "SubTAsk2",
            //     "",
            //     GestorTareas.Models.Task.TaskPriority.High,
            //     GestorTareas.Models.Task.TaskStatus.Pending,
            //     DateTime.Today.AddDays(31),
            //     0)
};



foreach(var t in taskList)
{
    TaskManager.AddTask(t);

}
// Console.WriteLine(TaskManager._taskList);
TaskSerializer<GestorTareas.Models.Task>.SerializateListTaskToJson(TaskManager._taskList);

IEnumerable<GestorTareas.Models.Task> deserializedTasks = TaskSerializer<GestorTareas.Models.Task>.DesSerializeJsonList();
var cont = 0;

foreach(var t in deserializedTasks)
{

    Console.WriteLine($"Nª: {++cont} __ "+t.ResumeTask()+$"\n");
}



using System.Formats.Tar;
using GestorTareas;
using GestorTareas.Models;
using Microsoft.VisualBasic;
// namespace GestorTareas;
// public class Program
// {

// public static void Main(string[]args){

// Console.WriteLine("Hello, World!");


// // var tasksList = new List<GestorTareas.Models.Task>
// // {
// // new SimpleTask("Revisar pull request","Descripcion", GestorTareas.Models.Task.TaskPriority.Normal,GestorTareas.Models.Task.TaskStatus.Pending, DateTime.Today.AddDays(1)),
// // new SimpleTask("Titulo","descripcion",GestorTareas.Models.Task.TaskPriority.Normal,GestorTareas.Models.Task.TaskStatus.InProgress,DateTime.Now.AddDays(18))
// // // new RecurringTask("Reunión de equipo","",GestorTareas.Models.Task.TaskPriority.Normal,GestorTareas.Models.Task.TaskStatus.Pending,DateTime.Today.AddDays(7),
// // // recurrenceRule:7),
// // // new TareaUrgente("Despliegue en producción",
// // // DateTime.Now.AddHours(3), PrioridadTarea.Alta, "Ana García")
// // };
// // var gestor = new GestorTareas.Models.TaskManager();
// // gestor.ShowResumeTask(tasksList);

// }
// }

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
                0)
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


    Console.Write(t.ResumeTask());
}

TaskSerializer.SerializateListTaskToJson()


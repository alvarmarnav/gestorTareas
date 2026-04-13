using System.Net;
using GestorTareas.Models;
using GestorTareas.Interfaces;
using System.Numerics;
namespace GestorTareas.Models;


public class Program{

// Console.WriteLine("Hello, World!");
private static void Main(string[]args){

Console.WriteLine("Entrando");
// Uso:
var tasksList = new List<Task>
{
new SimpleTask("Revisar pull request","Descripcion", Task.TaskPriority.Normal,Task.TaskStatus.Pending, DateTime.Today.AddDays(1)),
new SimpleTask("Titulo","descripcion",Task.TaskPriority.High,Task.TaskStatus.InProgress,DateTime.Now.AddDays(18))
// new RecurringTask("Reunión de equipo","",GestorTareas.Models.Task.TaskPriority.Normal,GestorTareas.Models.Task.TaskStatus.Pending,DateTime.Today.AddDays(7),
// recurrenceRule:7),
// new TareaUrgente("Despliegue en producción",
// DateTime.Now.AddHours(3), PrioridadTarea.Alta, "Ana García")
};
var gestor = new GestorTareas.Models.TaskManager();
gestor.ShowResumeTask(tasksList);

}
}
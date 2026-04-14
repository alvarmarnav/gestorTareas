using GestorTareas.Models;
namespace GestorTareas;
public class Program
{

public static void Main(string[]args){

Console.WriteLine("Hello, World!");


// var tasksList = new List<GestorTareas.Models.Task>
// {
// new SimpleTask("Revisar pull request","Descripcion", GestorTareas.Models.Task.TaskPriority.Normal,GestorTareas.Models.Task.TaskStatus.Pending, DateTime.Today.AddDays(1)),
// new SimpleTask("Titulo","descripcion",GestorTareas.Models.Task.TaskPriority.Normal,GestorTareas.Models.Task.TaskStatus.InProgress,DateTime.Now.AddDays(18))
// // new RecurringTask("Reunión de equipo","",GestorTareas.Models.Task.TaskPriority.Normal,GestorTareas.Models.Task.TaskStatus.Pending,DateTime.Today.AddDays(7),
// // recurrenceRule:7),
// // new TareaUrgente("Despliegue en producción",
// // DateTime.Now.AddHours(3), PrioridadTarea.Alta, "Ana García")
// };
// var gestor = new GestorTareas.Models.TaskManager();
// gestor.ShowResumeTask(tasksList);

}
}
using System;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Models;

public class CompositeTaskDTO : TaskDTO
{

    //TODO: pendiente subtasklist
    public List<SubTask> SubTasks {get;set;} = new List<SubTask>(30);
    
}

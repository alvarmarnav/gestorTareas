using System;
using GestorTareas.Models;

namespace GestorTareas.Application.DTOs;
public class CompositeTaskDTO : TaskDTO
{

    //TODO: pendiente subtasklist
    public int CompositeTaskType{get;set;} 

    public List<SubTask> SubTasks {get;set;} = new List<SubTask>(30);

    public List<LinkedTask> LinkedTaskList {get;set;} = new List<LinkedTask>(30);
    
}

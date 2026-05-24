using System;
using GestorTareas.Models;

namespace GestorTareas.Application.DTOs;
public class CreateCompositeTaskDto : TaskDTO
{
    public List<SubTask> SubTaskList {get;set;} = new List<SubTask>(30);
    
}

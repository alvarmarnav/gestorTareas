using System;

namespace GestorTareas.Models;

public class TaskManagerDto
{
    public List<TaskDTO> TaskList{get;set;} = new (60);

    public Dictionary<Guid, TaskDTO> TaskDictionary{get;set;} = new(60);

}

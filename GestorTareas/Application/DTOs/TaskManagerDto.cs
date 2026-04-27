using System;

namespace GestorTareas.Application.DTOs;

public class TaskManagerDto
{
    public List<TaskDTO> TaskList{get;set;} = new (60);

}

using System;

namespace GestorTareas.Models;

public class LinkedTaskDTO : TaskDTO
{
    //TODO: pendiente de terminar logica de dependencias de la Clase LinkedTask
    public List<LinkedTask> ListOfLinkedTasks { get; set; } = new(60);
    public int LinkedTaskOrder { get; set; }
}

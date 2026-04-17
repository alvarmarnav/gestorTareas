using System;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Models;

public class SubTaskDTO : TaskDTO
{
    public int SubTaskOrder {get;set;}
}

using System;
using GestorTareas.Models;

namespace GestorTareas.Application.DTOs;

public class SubTaskDTO : TaskDTO
{
        public int ParentCompositeTaskId { get; set; }
        public CompositeTask ParentCompositeTask{get;set;}

}

using System;
using System.ComponentModel.DataAnnotations;
using GestorTareas.Enums;
using GestorTareas.Models;

namespace GestorTareas.Application.DTOs;

public class ResponseSubTaskDto : TaskDTO
{
        public int ParentCompositeTaskId { get; set; }
        public ResponseCompositeTaskDto ParentCompositeTask { get; set; }
        public TaskType TaskType{get;set;}
}

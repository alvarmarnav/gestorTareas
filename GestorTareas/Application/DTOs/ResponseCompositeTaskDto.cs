using System;
using GestorTareas.Enums;
using GestorTareas.Models;

namespace GestorTareas.Application.DTOs;

public class ResponseCompositeTaskDto : TaskDTO
{
    public List<ResponseSubTaskDto> SubTasksList { get; set; } = [];
}

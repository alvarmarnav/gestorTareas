using System;
using GestorTareas.Enums;
using GestorTareas.Models;

namespace GestorTareas.Application.DTOs;

public class ResponseCollaborativeTaskDto : TaskDTO
{
    public List<TaskCollaboratorDto> TaskCollaborators { get; set; } = [];
}

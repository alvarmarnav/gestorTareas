using System;
using System.Security.Claims;
using GestorTareas.Application.DTOs;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Helpers;

public class UserConnectedHelper
{
    public static CurrentUserDto GetConnectedUser()
    {
        var currentUser = ClaimsPrincipal.Current;
        if (currentUser is null) throw new UnauthorizedAccessException($"Acceso denegado.");
        CurrentUserDto currentUserDto = new CurrentUserDto
        {
            CurrentUserId = int.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            CurrentUserRole = (CollaboratorRole)Enum.Parse(typeof(CollaboratorRole), currentUser.FindFirst(ClaimTypes.Role)?.Value)
        };
        return currentUserDto;
    }
}
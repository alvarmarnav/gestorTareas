using System;
using System.Security.Claims;
using GestorTareas.Application.DTOs;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Helpers;

public class UserConnectedHelper
{
    public static CurrentUserDto GetConnectedUser(ClaimsPrincipal currentUser)
    {
        var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUserRole = currentUser.FindFirst(ClaimTypes.Role)?.Value;

        if(!int.TryParse(currentUserId, out var userId))
        throw new UnauthorizedAccessException($"Acceso denegado.");

        if(!Enum.TryParse<CollaboratorRole>(currentUserRole, ignoreCase: true, out var userRole))
                throw new UnauthorizedAccessException($"Acceso denegado.");

        CurrentUserDto currentUserDto = new CurrentUserDto
        {
            CurrentUserId = userId,
            CurrentUserRole = userRole
        };
        return currentUserDto;
    }
}
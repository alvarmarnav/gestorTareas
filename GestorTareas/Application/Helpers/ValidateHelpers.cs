using System;
using GestorTareas.Application.DTOs;
using GestorTareas.Enums;
using Task = GestorTareas.Models.Task;

namespace GestorTareas;

public class ValidateHelpers
{
    private static bool IsAdmin(CurrentUserDto currentUserDto)
{
    return currentUserDto.CurrentUserSystemRole == SystemRole.Admin;
}

// private void ValidateCurrentUser(CurrentUserDto currentUserDto)
// {
//     if (currentUserDto is null)
//         throw new UnauthorizedAccessException("Usuario no autenticado.");

//     var user = _userRepository.GetUserById(currentUserDto.CurrentUserId)
//         ?? throw new KeyNotFoundException($"No existe ningún usuario con ID: {currentUserDto.CurrentUserId}.");

//     if (user.IsActive != true)
//         throw new UnauthorizedAccessException("Usuario inactivo.");
// }

private static void ValidateTaskCreationData(string title, DateTime? dueTime)
{
    if (string.IsNullOrWhiteSpace(title))
        throw new ArgumentException("El título es obligatorio.");

    if (title.Length > 30)
        throw new ArgumentException("El título no puede superar los 30 caracteres.");

    if (dueTime.HasValue && dueTime.Value <= DateTime.UtcNow)
        throw new ArgumentException("La fecha de vencimiento debe ser futura.");

    if (dueTime.HasValue && dueTime.Value > DateTime.UtcNow.AddYears(2))
        throw new ArgumentException("La fecha de vencimiento no debe ser mayor a 2 años.");
}

// private void EnsureCanManageTask(Task task, CurrentUserDto currentUserDto)
// {
//     if (IsAdmin(currentUserDto))
//         return;

//     if (task.UserId == currentUserDto.CurrentUserId)
//         return;

//     if (_repository.UserHasCollaboratorRole(task.Id, currentUserDto.CurrentUserId, CollaboratorRole.TaskAdministrator))
//         return;

//     throw new UnauthorizedAccessException("No tienes permiso para modificar esta tarea.");
// }
}

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


}

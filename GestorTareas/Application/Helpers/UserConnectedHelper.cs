using System;
using System.Security.Claims;
using GestorTareas.Enums;
using GestorTareas.Models;
using TaskStatus = GestorTareas.Enums.TaskStatus;

namespace GestorTareas.Helpers;

public class UserConnectedHelper
{
    public static int GetConnectedUser(ClaimsPrincipal user){
      var userIdStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdStr is null) throw new UnauthorizedAccessException();

        if(!int.TryParse(userIdStr,out int userId))
            throw new UnauthorizedAccessException();
        return userId;
    }
}
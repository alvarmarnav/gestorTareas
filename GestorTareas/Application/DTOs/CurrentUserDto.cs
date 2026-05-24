namespace GestorTareas.Application.DTOs;

public class CurrentUserDto
{
    public int CurrentUserId{get;set;}
    public Enums.CollaboratorRole CurrentUserTaskRole{get;set;}
    public Enums.SystemRole CurrentUserSystemRole{get;set;}
}
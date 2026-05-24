namespace GestorTareas.Application.DTOs;

public class CreateTaskCollaboratorDto
{
    
    public int UserId { get; set; }
    public Enums.CollaboratorRole CollaboratorRole { get; set; } = GestorTareas.Enums.CollaboratorRole.Viewer;
}
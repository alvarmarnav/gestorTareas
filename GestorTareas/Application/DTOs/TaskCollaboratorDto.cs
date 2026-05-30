namespace GestorTareas.Application.DTOs;

public class TaskCollaboratorDto
{
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public Enums.CollaboratorRole CollaboratorRole { get; set; } = GestorTareas.Enums.CollaboratorRole.Viewer;
}
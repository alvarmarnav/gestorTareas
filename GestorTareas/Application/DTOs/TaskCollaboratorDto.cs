namespace GestorTareas.Application.DTOs;

public class TaskCollaboratorDto
{
    public int TaskId { get; set; }
    // public CollaborativeTask Task{get;set;}
    public int UserId { get; set; }
    // public Models.User UserTask {get;set;}
    public Enums.CollaboratorRole CollaboratorRole { get; set; } = GestorTareas.Enums.CollaboratorRole.Viewer;
    // public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
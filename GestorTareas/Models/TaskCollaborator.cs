using GestorTareas.Enums;

namespace GestorTareas.Models;

public class TaskCollaborator
{
    public int TaskId { get; set; }
    public CollaborativeTask Task{get;set;} =null!;
    public int UserId { get; set; }
    public Models.User UserTask {get;set;}=null!;
    public Enums.CollaboratorRole CollaboratorRole { get; set; } = GestorTareas.Enums.CollaboratorRole.Viewer;
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}

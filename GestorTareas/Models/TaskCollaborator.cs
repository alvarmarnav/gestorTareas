using GestorTareas.Enums;

namespace GestorTareas.Models;

public class TaskCollaborator
{
    public int TaskId { get; set; }
    public CollaborativeTask Task{get;set;}
    public int UserId { get; set; }
    public Models.User UserTask {get;set;}
    public Enum CollaboratorRole { get; set; } = GestorTareas.Enums.CollaboratorRole.Viewer;
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public static explicit operator TaskCollaborator(User v)
    {
        throw new NotImplementedException();
    }
}

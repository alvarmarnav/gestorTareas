using System;
using GestorTareas.Interfaces;

namespace GestorTareas.Models;

public class CollaborativeTask : Task
{
    public List<User>? TeamMembers { get; set; }

    public CollaborativeTask(string title, string description, TaskPriority taskPriority, TaskStatus taskStatus, List<User> teamMembers) : base(title, description, TaskPriority.Normal, TaskStatus.Pending)
    {
        this.TeamMembers = teamMembers;
    }

    public void AddMember(Guid userId)
    {

    }

    public void RemoveMember(Guid userId) { }

    public List<User> GetMembers()
    {
        return this.TeamMembers;
    }




}

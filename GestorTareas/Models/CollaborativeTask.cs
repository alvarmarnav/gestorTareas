using System;
using GestorTareas.Interfaces;

namespace GestorTareas.Models;

public class CollaborativeTask : Task, IPostponeable, ICanBeStarted
{
    private List<User>? _TeamMembers;

    public double Progress{get;set;}

    public void AddMember(Guid userId)
    {
        
    }

    public List<User> GetMembers()
    {
        
    }

    public CollaborativeTask(string title, string description, TaskPriority taskPriority, TaskStatus taskStatus, List<User> teamMembers, double progress = 0) : base(title, description, TaskPriority.Normal, TaskStatus.Pending){
        this._TeamMembers = teamMembers;
        this.Progress = progress;
    }


}

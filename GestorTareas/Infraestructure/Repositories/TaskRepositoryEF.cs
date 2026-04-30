using System;
using System.Reflection.Metadata.Ecma335;
using GestorTareas.Interfaces;

namespace GestorTareas.Infraestructure.Repositories;

public class TaskRepositoryEF : ITaskRepository
{
    private readonly GestorTareasContext _context;

    public TaskRepositoryEF(GestorTareasContext context)
    {
        _context = context;
    }

    public List<Task> GetAllTasks(
        var tasks = _context.Tasks
        .Include(t => t.User)
        .Select(t => new
        {
            t.Id,
            t.Title,
            t.TaskDescription,
            t.Priority,
            t.Status,
            t.CreatedAt,
            t.UpdatedAt
        }).ToList();
    );

}

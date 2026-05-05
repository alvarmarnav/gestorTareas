using System;
using GestorTareas.Interfaces;
using GestorTareas.Models;
using Microsoft.EntityFrameworkCore;
using Task = GestorTareas.Models.Task;

namespace GestorTareas.Infraestructure.Repositories;

public class TaskRepositoryEF : ITaskRepository
{
    private readonly GestorTareasContext _context;

    public TaskRepositoryEF(GestorTareasContext context) => _context = context;

    public void AddTask(Task task)
    {
        _context.Add(task);
        _context.SaveChanges();
    }

    public void DeleteTask(Task task)
    {
        _context.Remove(task);
        _context.SaveChanges();
    }

    public List<Task> GetAllTasks()
    {
        return _context.Tasks.Include(t => t.User).ToList();
    }

    public Task? GetTaskById(Guid id) => _context.Tasks.Include(t => t.Id.Equals(id)).FirstOrDefault();

    public void UpdateTask(Task task)
    {
        _context.Tasks.Update(task);
        _context.SaveChanges();
    }
}

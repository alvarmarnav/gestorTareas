using System;
using System.Security.Claims;
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
    public List<Task> GetAllTasks(int userId)
    {
        return _context.Tasks.Include(t => t.User)
        .Where(t => t.User.Id == userId).ToList();
    }
    public Task? GetTaskById(int id) => _context.Tasks.Include(t => t.User).FirstOrDefault(t => t.Id.Equals(id));

    public void UpdateTask(Task task)
    {
        _context.Tasks.Update(task);
        _context.SaveChanges();
    }
}

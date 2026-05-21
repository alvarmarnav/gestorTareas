using GestorTareas.Enums;
using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Models;
using Microsoft.EntityFrameworkCore;

public class TaskSeeder
{
    private readonly GestorTareasContext _context;

    public TaskSeeder(GestorTareasContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task SeedAsync()
    {
        if (await _context.Tasks.AnyAsync())
            return;

        var users = await _context.Users.ToListAsync();

        var admin = users.FirstOrDefault();
        if (admin == null)
            throw new Exception("No users found. Run UsersSeeder first.");

        var user2 = users.Skip(1).FirstOrDefault();

        var tasks = new List<SimpleTask>
        {
            new SimpleTask
            {
                Title = "Preparar informe",
                UserId = admin.Id,
                TaskDescription = "Revisar métricas y generar informe.",
                Priority = TaskPriority.High,
                Status = GestorTareas.Enums.TaskStatus.InProgress,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DueTime = DateTime.UtcNow.AddDays(2)
            },

            new SimpleTask
            {
                Title = "Documentación API",
                UserId = admin.Id,
                TaskDescription = "Añadir ejemplos Swagger.",
                Priority = TaskPriority.High,
                Status = GestorTareas.Enums.TaskStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DueTime = DateTime.UtcNow.AddDays(5)
            },

            new SimpleTask
            {
                Title = "Bug crítico",
                UserId = user2?.Id ?? admin.Id,
                TaskDescription = "Error JWT autenticación.",
                Priority = TaskPriority.High,
                Status = GestorTareas.Enums.TaskStatus.InProgress,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DueTime = DateTime.UtcNow.AddDays(1)
            }
        };

        await _context.Tasks.AddRangeAsync(tasks);
        await _context.SaveChangesAsync();
    }
}
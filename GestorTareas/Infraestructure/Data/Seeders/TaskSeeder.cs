using System;
using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Models;
using GestorTareas.Enums;

namespace GestorTareas.Infraestructure.Data.Seeders;

public class TaskSeeder
{
    private readonly GestorTareasContext _context;

    public TaskSeeder(GestorTareasContext context) => _context = context;

    public async System.Threading.Tasks.Task AsyncSeeder()
    {
        await AsyncTaskSeeder();
    }

    public async System.Threading.Tasks.Task AsyncTaskSeeder()
    {
        if (_context.Tasks.Any() || _context.Tasks.Count() <= 20)
        {
            var tasksToAdd = new List<Models.Task>
            {
             new SimpleTask(){
                Title = "Preparar informe",
                UserId = 1,
    TaskDescription = "Revisar métricas y generar informe.",
    Priority = Enums.TaskPriority.High,
    Status = Enums.TaskStatus.InProgress,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(2),
    CancelReason = "Tarea no cancelada."
},
new SimpleTask()
{
    Title = "documentación API",
    UserId = 1,
    TaskDescription = "Añadir ejemplos Swagger.",
    Priority = Enums.TaskPriority.High,
    Status = Enums.TaskStatus.Pending,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(5),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Corregir bug",
    UserId = 2,
    TaskDescription = "Error de autenticación JWT.",
    Priority = Enums.TaskPriority.High,
    Status = Enums.TaskStatus.Completed,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(1),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Diseñar p. principal",
    UserId = 2,
    TaskDescription = "mockup responsive.",
    Priority = TaskPriority.Low,
    Status = Enums.TaskStatus.Pending,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(10),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Optimizar SQL",
    UserId = 2,
    TaskDescription = "Reducir tiempos de respuesta.",
    Priority = Enums.TaskPriority.High,
    Status = Enums.TaskStatus.InProgress,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(4),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "pruebas unitarias",
    UserId = 1,
    TaskDescription = "Cubrir servicios principales.",
    Priority = Enums.TaskPriority.Low,
    Status = Enums.TaskStatus.Pending,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(7),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Subir a producción",
    UserId = 1,
    TaskDescription = "Deploy de nueva versión.",
    Priority = Enums.TaskPriority.High,
    Status = Enums.TaskStatus.Pending,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(1),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "pull requests",
    UserId = 2,
    TaskDescription = "Validar código pendiente.",
    Priority = Enums.TaskPriority.Low,
    Status = Enums.TaskStatus.InProgress,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(3),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Implementar caché",
    UserId = 2,
    TaskDescription = "Mejorar rendimiento backend.",
    Priority = Enums.TaskPriority.High,
    Status = Enums.TaskStatus.Pending,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(6),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Actualizar dependencias",
    UserId = 1,
    TaskDescription = "Migrar paquetes NuGet.",
    Priority = Enums.TaskPriority.Low,
    Status = Enums.TaskStatus.Completed,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(8),
    CancelReason = "Tarea no cancelada."
},
new SimpleTask()
{
    Title = "Configurar entorno Docker",
    UserId = 1,
    TaskDescription = "Preparar contenedores para desarrollo.",
    Priority = Enums.TaskPriority.High,
    Status = Enums.TaskStatus.InProgress,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(3),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Refactorizar controlador",
    UserId = 2,
    TaskDescription = "Separar lógica de negocio.",
    Priority = Enums.TaskPriority.Normal,
    Status = Enums.TaskStatus.Pending,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(5),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Implementar logs",
    UserId = 1,
    TaskDescription = "Añadir logging centralizado.",
    Priority = Enums.TaskPriority.High,
    Status = Enums.TaskStatus.Completed,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(2),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Crear endpoint",
    UserId = 2,
    TaskDescription = "Mostrar tareas completadas y pendientes.",
    Priority = Enums.TaskPriority.High,
    Status = Enums.TaskStatus.InProgress,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(6),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Corregir validaciones",
    UserId = 1,
    TaskDescription = "Validar emails y campos obligatorios.",
    Priority = Enums.TaskPriority.High,
    Status = Enums.TaskStatus.Pending,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(1),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Diseñar dashboard admin",
    UserId = 2,
    TaskDescription = "Crear panel principal administrativo.",
    Priority = Enums.TaskPriority.Low,
    Status = Enums.TaskStatus.Pending,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(9),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Optimizar consultas LINQ",
    UserId = 1,
    TaskDescription = "Reducir uso innecesario de memoria.",
    Priority = Enums.TaskPriority.High,
    Status = Enums.TaskStatus.InProgress,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(4),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Actualizar README",
    UserId = 2,
    TaskDescription = "Documentar instalación y despliegue.",
    Priority = Enums.TaskPriority.Low,
    Status = Enums.TaskStatus.Completed,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(7),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Añadir paginación",
    UserId = 1,
    TaskDescription = "Paginar resultados de tareas.",
    Priority = Enums.TaskPriority.Normal,
    Status = Enums.TaskStatus.Pending,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(5),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "estilos responsive",
    UserId = 2,
    TaskDescription = "Adaptar interfaz a móviles.",
    Priority = Enums.TaskPriority.Low,
    Status = Enums.TaskStatus.InProgress,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(4),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "servicio de notificaciones",
    UserId = 1,
    TaskDescription = "Enviar avisos de tareas vencidas.",
    Priority = Enums.TaskPriority.High,
    Status = Enums.TaskStatus.Pending,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(3),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "arquitectura proyecto",
    UserId = 2,
    TaskDescription = "Evaluar separación por capas.",
    Priority = Enums.TaskPriority.Low,
    Status = Enums.TaskStatus.Completed,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(10),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Configurar AutoMapper",
    UserId = 1,
    TaskDescription = "Mapear entidades y DTOs.",
    Priority = Enums.TaskPriority.Low,
    Status = Enums.TaskStatus.Pending,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(2),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "Eliminar código obsoleto",
    UserId = 2,
    TaskDescription = "Limpiar clases sin uso.",
    Priority = Enums.TaskPriority.Low,
    Status = Enums.TaskStatus.InProgress,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(6),
    CancelReason = "Tarea no cancelada."
},

new SimpleTask()
{
    Title = "filtros de búsqueda",
    UserId = 1,
    TaskDescription = "Filtrar tareas por estado y prioridad.",
    Priority = Enums.TaskPriority.High,
    Status = Enums.TaskStatus.Pending,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    DueTime = DateTime.Now.AddDays(5),
    CancelReason = "Tarea no cancelada."
}
};
            await _context.Tasks.AddRangeAsync(tasksToAdd);
            await _context.SaveChangesAsync();
        }

    }

}

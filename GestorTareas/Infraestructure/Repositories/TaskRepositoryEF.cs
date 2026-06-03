using System;
using System.Security.Claims;
using GestorTareas.Enums;
using GestorTareas.Interfaces;
using GestorTareas.Models;
using Microsoft.EntityFrameworkCore;
using Task = GestorTareas.Models.Task;
namespace GestorTareas.Infraestructure.Repositories;

public class TaskRepositoryEF : ITaskRepository
{
    private readonly GestorTareasContext _context;

    public TaskRepositoryEF(GestorTareasContext context) => _context = context;

    public Task CreateTask(Task task)
    {
        _context.Add(task);
        _context.SaveChanges();
        return task;
    }

    public void DeleteTask(Task task)
    {
        _context.Tasks.Remove(task);
        _context.SaveChanges();
    }
    public List<Task> GetAllTasks()
    {
        return _context.Tasks.Include(t => t.User).ToList();
    }

    public List<Task> GetAllTasksByUser(int userId)
    {
        return _context.Tasks.Include(t => t.User)
        .Where(t => t.UserId == userId).ToList();
    }

    public Task? GetTaskById(int id)
    {
        var task = _context.Tasks
    .Include(t => t.User)
    .Include(t => t.Dependencies)
    .ThenInclude(d => d.DependsOnTask)
    .Include(t => t.RequiredByOtherTask)
    .FirstOrDefault(t => t.Id == id);

        if (task is CompositeTask)
        {
            return _context.CompositeTasks
                .Include(t => t.User)
                .Include(t => t.SubTaskList)
                .Include(t => t.Dependencies)
                    .ThenInclude(d => d.DependsOnTask)
                .Include(t => t.RequiredByOtherTask)
                .FirstOrDefault(t => t.Id == id);
        }

        if (task is CollaborativeTask)
        {
            return _context.CollaborativeTasks
                .Include(t => t.User)
                .Include(t => t.TaskCollaborators)
                    .ThenInclude(tc => tc.UserTask)
                .Include(t => t.Dependencies)
                    .ThenInclude(d => d.DependsOnTask)
                .Include(t => t.RequiredByOtherTask)
                .FirstOrDefault(t => t.Id == id);
        }
        return task;
    }

    public (List<Task> tasks, int total) GetTotalPaginated(int page, int itemsPerPage, int userId, bool? onlyCompletedTask = null,
string? search = null)
    {
        User userConsultant = _context.Users.FirstOrDefault(u => u.Id == userId) ?? throw new KeyNotFoundException($"No existe el usuario con el ID: {userId}");

        // Consulta base — todavía no va a SQL  
        var query = _context.Tasks
        .Include(t => t.User)
        .Include(t => t.Dependencies)
        .Include(t => t.RequiredByOtherTask)
        .Include(t => ((CollaborativeTask)t).TaskCollaborators)
        .ThenInclude(tc => tc.UserTask)
        .AsQueryable();
        //Con esto temporalmente seriviria para hacerle llegar al otro usuario colaborador las tareas donde se le añade, aunque lo ideal seria una NOTIFICACION
        if (!(bool)userConsultant.IsAdmin)
        {
            query = query.Where(t =>
                t.UserId == userId ||
                _context.TaskCollaborators.Any(tc =>
                    tc.TaskId == t.Id &&
                    tc.UserId == userId));
        }
        // Aplicar filtros solo si se han especificado
        if (onlyCompletedTask.HasValue && onlyCompletedTask.Value == true)
            query = query.Where(t => t.TaskStatus == Enums.TaskStatus.Completed);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(t => t.Title.Contains(search));

        var total = query.Count();
        var tasks = query
        .OrderBy(t => t.CreatedAt)
        .Skip((page - 1) * itemsPerPage)
        .Take(itemsPerPage)
        .ToList();

        return (tasks, total);
    }
    public void UpdateTask(Task task)
    {
        _context.Tasks.Update(task);
        _context.SaveChanges();
    }
    public void AddTaskCollaborator(CollaborativeTask collaborativeTask, TaskCollaborator tcollaborator)
    {
        _context.TaskCollaborators.Add(tcollaborator);
        _context.SaveChanges();
    }
    public void RemoveCollaborator(CollaborativeTask collaborativeTask, TaskCollaborator tcollaborator)
    {
        _context.TaskCollaborators.Remove(tcollaborator);
        _context.SaveChanges();
    }

    public bool ExistsCircularRelation(int taskId, int dependsOnTaskId)
    {
        var visited = new HashSet<int>();
        var stack = new Stack<int>();

        stack.Push(dependsOnTaskId);

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if (current == taskId)
                return true;

            if (!visited.Add(current))
                continue;

            var next = _context.LinkedTasks.Where(lt => lt.TaskId == current)
                .Select(lt => lt.DependsOnTaskId)
                .ToList();

            foreach (var nextId in next)
                stack.Push(nextId);
        }

        return false;
    }
    public bool ExistsLinkedRelation(int taskId, int dependsOnTaskId)
    {
        return _context.LinkedTasks.Any(lt => lt.TaskId == taskId && lt.DependsOnTaskId == dependsOnTaskId);
    }
    public LinkedTask AddLinkedRelation(LinkedTask linkedTask)
    {
        _context.LinkedTasks.Add(linkedTask);
        _context.SaveChanges();
        return linkedTask;
    }

    public CollaborativeTask? GetCollaborativeTaskById(int collTaskId)
    {
        return _context.CollaborativeTasks
        .Include(ct => ct.TaskCollaborators)
        .ThenInclude(tc => tc.UserTask)
        .Include(ct => ct.User)
        .FirstOrDefault(ct => ct.Id == collTaskId);
    }
    public CompositeTask? GetCompositeTaskById(int taskId)
    {
        return _context.CompositeTasks
        .Include(ct => ct.User)
        .Include(ct => ct.SubTaskList)
        .FirstOrDefault(ct => ct.Id == taskId);
    }
    public bool UserHasCollaboratorRole(int taskId, int userId, CollaboratorRole role)
    {
        return _context.TaskCollaborators.Any(tc =>
            tc.TaskId == taskId &&
            tc.UserId == userId &&
            tc.CollaboratorRole == role);
    }
    public List<Task> GetLinkableTasks(int userId, bool includeAllUsers, int? excludeTaskId = null)
    {
        var query = _context.Tasks
            .Include(t => t.User)
            .Where(t => t.TaskType == TaskType.SimpleTask)
            .AsQueryable();

        if (!includeAllUsers)
        {
            query = query.Where(t => t.UserId == userId);
        }

        if (excludeTaskId.HasValue)
        {
            var baseTaskId = excludeTaskId.Value;

            var alreadyLinkedTaskIds = _context.LinkedTasks
                .Where(lt => lt.TaskId == baseTaskId || lt.DependsOnTaskId == baseTaskId)
                .Select(lt => lt.TaskId == baseTaskId ? lt.DependsOnTaskId : lt.TaskId)
                .ToList();

            query = query.Where(t =>
                t.Id != baseTaskId &&
                !alreadyLinkedTaskIds.Contains(t.Id));
        }

        return query
            .OrderBy(t => t.Title)
            .ToList();
    }

    public TaskCollaborator? GetAllTaskCollaborators(int taskId, int currentUserId)
    {
        return _context.TaskCollaborators.FirstOrDefault(tc => tc.UserId == currentUserId && tc.TaskId == taskId);
    }

    public void CompleteTask(Task taskToComplete)
    {
        _context.Tasks.Update(taskToComplete);
        _context.SaveChanges();
    }

    public Task? GetTaskByIdWithRelations(int taskId)
    {
        var task = _context.Tasks
         .Include(t => t.User)
         .Include(t => t.Dependencies)
         .ThenInclude(d => d.DependsOnTask)
         .Include(t => t.RequiredByOtherTask)
         .FirstOrDefault(t => t.Id == taskId);

        if (task is CompositeTask)
        {
            return _context.CompositeTasks
                .Include(t => t.User)
                .Include(t => t.SubTaskList)
                .Include(t => t.Dependencies)
                    .ThenInclude(d => d.DependsOnTask)
                .Include(t => t.RequiredByOtherTask)
                .FirstOrDefault(t => t.Id == taskId);
        }

        if (task is CollaborativeTask)
        {
            return _context.CollaborativeTasks
                .Include(t => t.User)
                .Include(t => t.TaskCollaborators)
                    .ThenInclude(tc => tc.UserTask)
                .Include(t => t.Dependencies)
                    .ThenInclude(d => d.DependsOnTask)
                .Include(t => t.RequiredByOtherTask)
                .FirstOrDefault(t => t.Id == taskId);
        }
        return task;
    }

    public List<Task> GetAllTaskLinked(int taskId)
    {
        var linked = _context.LinkedTasks
       .Include(t => t.Task)              // tareas que dependen de esta
       .Include(t => t.DependsOnTask)     // tareas de las que depende
       .Where(t => t.TaskId == taskId || t.DependsOnTaskId == taskId)
       .ToList();

        // Extraemos ambas listas sin duplicados
        var result = linked
            .SelectMany(t => new[] { t.Task, t.DependsOnTask })
            .Where(t => t != null && t.Id != taskId)
            .Select(t => t!)
            .Distinct()
            .OrderBy(t => t.CreatedAt)
            .ToList();

        return result;
    }

    public LinkedTask? GetLinkedRelationById(int linkedTaskId)
    {
        return _context.LinkedTasks
            .Include(r => r.Task)
            .Include(r => r.DependsOnTask)
            .FirstOrDefault(r => r.Id == linkedTaskId);
    }

    public void DeleteLinkedRelation(LinkedTask linkedTask)
    {
        _context.LinkedTasks.Remove(linkedTask);
        _context.SaveChanges();
    }

    public bool AlreadyExistsCollaborator(int taskId, int userId)
    {
        return _context.TaskCollaborators.Any(tc => tc.TaskId == taskId && tc.UserId == userId);
    }
    public void RemoveTaskCollaborator(CollaborativeTask collaborativeTask, TaskCollaborator tcollaborator)
    {
        _context.TaskCollaborators.Remove(tcollaborator);
        _context.SaveChanges();
    }
    public RecurringTask? GetRecurringTaskById(int taskId)
    {
        return _context.RecurringTasks
            .Include(t => t.User)
            .Include(t => t.Dependencies)
                .ThenInclude(d => d.DependsOnTask)
            .Include(t => t.RequiredByOtherTask)
            .FirstOrDefault(t => t.Id == taskId);
    }
    public List<RecurringTask> GetRecurringTasksBySeriesId(Guid recurringSeriesId)
    {
        return _context.RecurringTasks
            .Where(t => t.RecurringSeriesId == recurringSeriesId)
            .OrderBy(t => t.RecurringTasksCount)
            .ThenBy(t => t.DueTime)
            .ToList();
    }
    public List<SubTask> GetSubTasksByCompositeTaskId(int compositeTaskId)
    {
        return _context.SubTasks
            .Where(st => st.ParentCompositeTaskId == compositeTaskId)
            .OrderBy(st => st.CreatedAt)
            .ToList();
    }
    public List<LinkedTask> GetLinkedRelationsByTaskId(int taskId)
    {
        return _context.LinkedTasks
            .Include(lt => lt.Task)
            .Include(lt => lt.DependsOnTask)
            .Where(lt => lt.TaskId == taskId || lt.DependsOnTaskId == taskId)
            .OrderBy(lt => lt.LinkedTaskOrder)
            .ThenBy(lt => lt.Id)
            .ToList();
    }
    public List<Task> GetTasksWithLinkedRelations(int userId, bool includeAllUsers)
    {
        var query = _context.Tasks
            .Include(t => t.User)
            .Include(t => t.Dependencies)
            .Include(t => t.RequiredByOtherTask)
            .Where(t => t.Dependencies.Any() || t.RequiredByOtherTask.Any())
            .AsQueryable();

        if (!includeAllUsers)
        {
            query = query.Where(t => t.UserId == userId);
        }

        return query
            .OrderBy(t => t.CreatedAt)
            .ToList();
    }

}

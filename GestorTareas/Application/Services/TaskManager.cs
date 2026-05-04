using System;
using GestorTareas.Application.DTOs;
using GestorTareas.Infraestructure.Repositories;

namespace GestorTareas.Application.Services;

public class TaskManager
{

    private List<Models.Task> TaskList { get; set; } = new(60);

    private Dictionary<Guid, Models.Task> TaskDictionary { get; set; } = new(60);

    public TaskRepository Repository { get; set; }

    public TaskManager(TaskRepository repository)
    {
        TaskList = new(60);
        TaskDictionary = new Dictionary<Guid, Models.Task>(60);
        Repository = repository;
        LoadRepository();
    }

    public void LoadRepository()
    {
        var listTasksDto = Repository.Load();
        TaskList = listTasksDto.TaskList.Select(DtoManager.DtoToTask).ToList();
        TaskDictionary = TaskList.ToDictionary(t => t.Id);
    }

    public void SaveRepository()
    {

        var listTasksDto = new TaskManagerDto()
        {
            TaskList = TaskList.Select(DtoManager.TaskToDto).ToList(),
        };
        Repository.Save(listTasksDto);
    }

    public void AddTask(Models.Task item)
    {

        ArgumentNullException.ThrowIfNull(item);

        if (TaskList.Exists(i => i.Title.Equals(item.Title, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("No se puede añadir. Ya existe una tarea con el mismo título.");

        TaskList.Add(item);

        if (!TaskDictionary.TryAdd(item.Id, item))
        {
            throw new ArgumentException("$La tarea con el id: {item.Id} ya existe en el diccionario.");
        }
    }

    public IReadOnlyList<Models.Task> ShowAllItems()
    {
        IReadOnlyList<Models.Task> readOnlyItemList = TaskList;
        return readOnlyItemList;
    }

    public Models.Task? IdSearch(Guid id)
    {

        if (!TaskDictionary.TryGetValue(id, out Models.Task? item))
        {
            throw new KeyNotFoundException($"No se encontró una tarea con el id: {id}");
        }
        return item;

    }

    public void RemoveTask(Guid id)
    {

        if (!TaskDictionary.Remove(id))
        {
            foreach (var i in TaskDictionary)
            {
                Console.WriteLine($"Valor id: {i.Key} valor de valor: {i.Value}");
            }
            throw new KeyNotFoundException($"La id: {id} no se encuentra en el diccionario.");
        }

        TaskList.RemoveAll(t => t.Id == id);
    }

    public void ShowResumeAllTasks(IEnumerable<Models.Task> taskList)
    {
        foreach (var t in taskList)
        {

            Console.WriteLine(t.ResumeTask());
        }
    }

    //TODO: REVISAR ESTE METODO
    public IEnumerable<Models.Task> GenericTaskSearch(Func<Models.Task, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);



        return TaskList.Where(condition);
    }
}

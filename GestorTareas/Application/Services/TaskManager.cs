using System;
using GestorTareas.Application.DTOs;
using GestorTareas.Infraestructure.Repositories;

namespace GestorTareas.Application.Services;

public class TaskManager
{

    private List<GestorTareas.Models.Task> TaskList { get; set; } = new(60);

    private Dictionary<Guid, GestorTareas.Models.Task> TaskDictionary { get; set; } = new(60);

    public TaskRepository Repository { get; set; }

    public TaskManager(TaskRepository repository)
    {
        TaskList = new(60);
        TaskDictionary = new Dictionary<Guid, GestorTareas.Models.Task>(60);
        Repository = repository;
        LoadRepository();
    }

    public void LoadRepository()
    {
        var listTasksDto = Repository.Load();
        TaskList = listTasksDto.TaskList.Select(DtoManager.DtoToTask).ToList();
        //Evitar Duplicados
        // TaskDictionary = listTasksDto.TaskDictionary.ToDictionary(
        //     keyVal =>keyVal.Key,
        //     keyVal => DtoManager.DtoToTask(keyVal.Value)
        // );
        TaskDictionary = TaskList.ToDictionary(t => t.Id);
    }

    public void SaveRepository()
    {

        var listTasksDto = new TaskManagerDto()
        {
            TaskList = TaskList.Select(DtoManager.TaskToDto).ToList(),
            //Evitar Duplicados
            // TaskDictionary = TaskDictionary.ToDictionary(
            //     keyval => keyval.Key,
            //     keyval => DtoManager.TaskToDto(keyval.Value)
            // )
        };
        Repository.Save(listTasksDto);
    }

    public void AddTask(GestorTareas.Models.Task item)
    {

        ArgumentNullException.ThrowIfNull(item);

        if (TaskList.Exists(i => i.Title.Equals(item.Title, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("No se puede añadir. Ya existe una tarea con el mismo título.");

        TaskList.Add(item);

        if (!TaskDictionary.TryAdd(item.Id, item))
        {
            throw new ArgumentException("$La tarea con el id: {item.Id} ya existe en el diccionario.");
        }

        //TODO: ELIMINAR ESTE CONSOLE.WRITE
        // Console.WriteLine($"Tarea '{item.Title}' añadida con éxito.");
    }

    public IReadOnlyList<GestorTareas.Models.Task> ShowAllItems()
    {
        IReadOnlyList<GestorTareas.Models.Task> readOnlyItemList = TaskList;
        return readOnlyItemList;
    }

    public GestorTareas.Models.Task? IdSearch(Guid id)
    {

        if (!TaskDictionary.TryGetValue(id, out GestorTareas.Models.Task? item))
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
        //TODO: ELIMINAR ESTE CONSOLE.WRITE
        Console.WriteLine($"Tarea con id: {id} eliminada con éxito.");

    }

    public void ShowResumeAllTasks(IEnumerable<GestorTareas.Models.Task> taskList)
    {
        foreach (var t in taskList)
        {

            Console.WriteLine(t.ResumeTask());
        }
    }

    //TODO: REVISAR ESTE METODO
    public IEnumerable<GestorTareas.Models.Task> GenericTaskSearch(Func<GestorTareas.Models.Task, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);



        return TaskList.Where(condition);
    }

    // public void SaveTasksToJson(string filePath)
    // {
    //     TaskSerializer<Task>.SerializateListTaskToJson(TaskList, filePath);
    // }
}

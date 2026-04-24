using System;
using System.ComponentModel.Design;
using GestorTareas.Interfaces;

namespace GestorTareas.Models;

public class TaskManager
{

    private List<Task> TaskList{get;set;} = new(60);
    
    private Dictionary<Guid, Task> TaskDictionary{get;set;} = new(60);

    public TaskRepository Repository {get;set;}

    public TaskManager(TaskRepository repository)
    {
        TaskList = new (60);
        TaskDictionary = new Dictionary<Guid, Task>(60);
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

    public void AddTask(Task item)
    {

        ArgumentNullException.ThrowIfNull(item);

        

        TaskList.Add(item);

        if (!TaskDictionary.TryAdd(item.Id, item))
        {
            throw new ArgumentException("$La tarea con el id: {item.Id} ya existe en el diccionario.");
        }

        //TODO: ELIMINAR ESTE CONSOLE.WRITE
        // Console.WriteLine($"Tarea '{item.Title}' añadida con éxito.");
    }

    public IReadOnlyList<Task> ShowAllItems()
    {
        IReadOnlyList<Task> readOnlyItemList = TaskList;
        return readOnlyItemList;
    }

    public Task? IdSearch(Guid id)
    {
        
        if (!TaskDictionary.TryGetValue(id, out Task? item))
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

    public void ShowResumeAllTasks(IEnumerable<Task> taskList)
    {
        foreach (var t in taskList)
        {

            Console.WriteLine(t.ResumeTask());
        }
    }

//TODO: REVISAR ESTE METODO
    public IEnumerable<Task> GenericTaskSearch(Func<Task, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);



        return TaskList.Where(condition);
    }

    // public void SaveTasksToJson(string filePath)
    // {
    //     TaskSerializer<Task>.SerializateListTaskToJson(TaskList, filePath);
    // }

}

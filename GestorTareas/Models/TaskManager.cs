using System;
using System.ComponentModel.Design;
using GestorTareas.Interfaces;

namespace GestorTareas.Models;

public class TaskManager<T> where T : class, IIdentificable, ITaskDisplayable
{

    protected static readonly List<T> _taskList = new(60);
    protected static readonly Dictionary<Guid, T> _taskDictionary = new(60);
    public static void AddTask(T item)
    {

        ArgumentNullException.ThrowIfNull(item);

        _taskList.Add(item);

        if (!_taskDictionary.TryAdd(item.Id, item))
        {
            throw new ArgumentException("$La tarea con el id: {item.Id} ya existe en el diccionario.");
        }

        //TODO: ELIMINAR ESTE CONSOLE.WRITE
        // Console.WriteLine($"Tarea '{item.Title}' añadida con éxito.");
    }

    public IReadOnlyList<T> ShowAllItems()
    {
        IReadOnlyList<T> readOnlyItemList = _taskList;
        return readOnlyItemList;
    }

    public T? IdSearch(Guid id)
    {
        
        if (!_taskDictionary.TryGetValue(id, out T? item))
        {
            throw new KeyNotFoundException($"No se encontró una tarea con el id: {id}");
        }
       return item;

    }

    public void RemoveTask(Guid id)
    {
        if (!_taskDictionary.Remove(id))
        {
            throw new KeyNotFoundException($"La id: {id} no se encuentra en el diccionario.");
        }
        //TODO: ELIMINAR ESTE CONSOLE.WRITE
        Console.WriteLine($"Tarea con id: {id} eliminada con éxito.");

    }

    public void ShowResumeAllTasks(IEnumerable<T> taskList)
    {
        foreach (T t in taskList)
        {

            Console.WriteLine(t.ResumeTask());
        }
    }

//TODO: REVISAR ESTE METODO
    public IEnumerable<T> GenericTaskSearch(Func<T, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);



        return _taskList.Where(condition);
    }

}

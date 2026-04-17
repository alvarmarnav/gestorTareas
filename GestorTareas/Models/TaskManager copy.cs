using System;
using System.ComponentModel.Design;

namespace GestorTareas.Models;

public class TaskManager
{

    public static List<Task> _taskList = new List<Task>();
    public static Dictionary<Guid, Task> _taskDictionary = new Dictionary<Guid,Task>();
    public static void AddTask(Task task)
    {
        
        _taskList.Add(task);

        //Añadir al Dictionary
        _taskDictionary.Add(task.Id,task);

        //TODO: ELIMINAR ESTE CONSOLE.WRITE
        Console.WriteLine($"Tarea '{task.Title}' añadida con éxito.");
    }
    
     public IReadOnlyList<Task> ShowAllTasks()
    {
        IReadOnlyList<Task> readOnlyTaskList =  _taskList;
        return readOnlyTaskList;
    }

    public Task IdSearch(Guid id)
    {
        var searchTask = (Task)_taskDictionary.
            Where(t=>t.Key == id)
            .Select(t=> t.Key ==id);
            
        return searchTask;

    // var tasks = _taskDictionary
    // .Where(t=> t.Key == id)
    // .Select(t=> t.Key==id);
    }

    public void ShowResumeTask(IEnumerable<Task> taskList)
    {
        foreach (Task t in taskList)
        {
            Console.WriteLine(t.ResumeTask());
        }
    }

    public IEnumerable<Task> GenericTaskSearch(Func<Task,bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);
        
        return _taskList.Where(condition);
    }

}

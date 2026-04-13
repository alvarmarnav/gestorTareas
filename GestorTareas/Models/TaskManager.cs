using System;

namespace GestorTareas.Models;

public class TaskManager
{

    public void AddTask(Task task)
    {
        
    }
    public List<Task> ListAllTask()
    {
        List<Task> taskList = new List<Task>(10);

        return taskList;
    }

    // public Task IdSearch(Guid id)
    // {
    //     return ;
    // }

    public void ShowResumeTask(IEnumerable<Task> taskList)
    {
        foreach (Task t in taskList)
        {
            Console.WriteLine(t.ResumeTask());
        }
    }
}

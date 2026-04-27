using System;

namespace GestorTareas.Application.Services;

using GestorTareas.Models;

public class TaskSummaryManager
{
    public void ResumeTask(Task itemTask)
    {
       string resumeString = itemTask.ResumeTask();

       Console.WriteLine(resumeString);
    }
}
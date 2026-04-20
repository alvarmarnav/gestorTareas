using System;

namespace GestorTareas.Models;

public class LinkedTaskContainer
{

    public Guid Id{get;set;}
    public string? ContainerDescription
    {
        get; set
        {
            field = value??$"Contenedor {Id}: {value}";
        }
    }
    public List<LinkedTask> LinkedTaskList {get;set;} = new(10);


    public LinkedTaskContainer(
       string containerDescription
    )
    {
        Id = Guid.NewGuid();
        ContainerDescription = containerDescription;
        LinkedTaskList = new(10);
    }

    public double? CalculateProgress()
    {

        if(LinkedTaskList.Count()<=0)
            return 0;

        int completedTask = 0;

        foreach (var t in LinkedTaskList)
        {
            if(t.Status == Task.TaskStatus.Completed)
                completedTask++;
            // if(t.Status == Task.TaskStatus.Cancelled)
            //     completedTask++;
        }

        return (completedTask*100)/LinkedTaskList.Count();
    }


}

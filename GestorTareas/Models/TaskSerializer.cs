using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace GestorTareas.Models;

public class TaskSerializer
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
       WriteIndented = true,
       DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
       PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
    };

    public static void SerializateListTaskToJson(List<Task> taskList)
    {
        // return JsonSerializer.Serialize(taskList, _jsonOptions);
        var json = JsonSerializer.Serialize(taskList,_jsonOptions);
        File.WriteAllText("taskList.json",json);

    }

    public static List<Task> DesSerializeJsonList()
    {

        if(File.Exists("listTask.json")){
            var loaded = JsonSerializer.Deserialize<List<Task>>((string)File.ReadAllText("taskList.json"));
            
            if(loaded is not null){
                foreach (var t in loaded)
                    {
                    Console.WriteLine(t.Title);
                    }
                return loaded??throw new NullReferenceException("Contenido null");
            }
            else
            {
                throw new Exception("Lista vacia.");
            }
    }
        else
        {
            throw new FileNotFoundException("No se encuentra el archivo *.json");
        }
    }


}

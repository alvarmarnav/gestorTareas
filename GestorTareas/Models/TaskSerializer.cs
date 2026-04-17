using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using GestorTareas.Interfaces;
using Microsoft.VisualBasic;

namespace GestorTareas.Models;

public class TaskSerializer<T> where T : class, IIdentificable
{
    private static JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        IncludeFields = true,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    public static void SerializateListTaskToJson(IEnumerable<T> taskList)
    {
        
        var json = JsonSerializer.Serialize(taskList, _jsonOptions);
        File.WriteAllText("taskList.json", json);

    }

    public static IEnumerable<T> DesSerializeJsonList()
    {
        if (File.Exists("taskList.json"))
        {

            IEnumerable<T>? loaded = JsonSerializer.Deserialize<List<T>>(File.ReadAllText("taskList.json"),_jsonOptions);

            // Console.WriteLine("Serializando lista de tareas a JSON...");
            if (loaded is not null && loaded.Count() > 0)
            {
                // Console.WriteLine("Deserializando lista de tareas desde JSON...");

                // foreach (var t in loaded)
                // {
                //     Console.WriteLine(t.Title);
                // }
                return loaded ?? throw new NullReferenceException("Lista null");
            }
            else
            {
                throw new NullReferenceException("Lista vacia.");
            }
        }
        else
        {
            throw new FileNotFoundException("No se encuentra el archivo *.json");
        }
    }


}

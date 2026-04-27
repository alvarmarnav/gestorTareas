using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using GestorTareas.Interfaces;
using Microsoft.VisualBasic;

namespace GestorTareas.Models;

public class TaskSerializer
{
    private static JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        IncludeFields = true,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    public static void SerializateListTaskToJson(TaskManagerDto taskManagerDto, string filePath)
    {
        
        if(string.IsNullOrWhiteSpace(filePath) || Path.IsPathFullyQualified(filePath) || Path.GetInvalidPathChars().Any(c=>filePath.Contains(c)))
            throw new ArgumentException("Ruta no válida.");

        var json = JsonSerializer.Serialize(taskManagerDto, _jsonOptions);
        File.WriteAllText(filePath, json);

    }

    public static TaskManagerDto DeserializeJsonList(string filePath)
    {
        if (File.Exists(filePath))
        {

            var loaded = JsonSerializer.Deserialize<TaskManagerDto>(File.ReadAllText(filePath),_jsonOptions);

            // Console.WriteLine("Serializando lista de tareas a JSON...");
            if (loaded is not null)
            {                
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GestorTareas.Interfaces;
namespace GestorTareas.Models;

public class TaskRepository
{
    private readonly string _filePath = "taskList.json";
    private JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        IncludeFields = true,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    public TaskRepository() { }
    public TaskManagerDto Load()
    {
        if (!File.Exists(_filePath))
        {
            return new TaskManagerDto();
        }
        else
        {
            try
            {
                string json = File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new TaskManagerDto();
                }

                TaskManagerDto? loaded = JsonSerializer.Deserialize<TaskManagerDto>(json, _jsonOptions);
                return loaded ?? new TaskManagerDto();
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Error deserializando JSON: {e}");
                return new TaskManagerDto();
            }
        }
    }

    public void Save(TaskManagerDto taskManagerDto)
    {
        var json = JsonSerializer.Serialize(taskManagerDto, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }

    public static void AutoSave() { }

}

using System.ComponentModel.DataAnnotations;
using Microsoft.Testing.Platform.OutputDevice;

namespace GestorTareas.Application.DTOs;

public class CreateLinkedTaskRelationDto
{
    [Range(1,int.MaxValue, ErrorMessage="La tarea de la que depende debe ser mayor de 0.")]
    public int DependsOnTaskId{get;set;}
    [Range(1,int.MaxValue,ErrorMessage ="El orden debe ser mayor que 0.")]
    public int LinkedTaskOrder{get;set;}
}
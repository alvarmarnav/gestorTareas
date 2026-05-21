using System;
using GestorTareas.Models;

namespace GestorTareas.Application.DTOs;

public class LinkedTaskDTO
{
    public int Id{get;set;}
    public int TaskId{get;set;}
    public int DependsOnTaskId{get;set;}
    public int LinkedTaskOrder { get; set; }
}

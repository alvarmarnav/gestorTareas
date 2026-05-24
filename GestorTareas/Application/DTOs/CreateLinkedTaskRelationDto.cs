namespace GestorTareas.Application.DTOs;

public class CreateLinkedTaskRelationDto
{
    public int DependsOnTaskId{get;set;}
    public int LinkedTaskOrder{get;set;}
}
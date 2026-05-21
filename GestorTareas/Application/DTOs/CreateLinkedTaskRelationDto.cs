namespace GestorTareas.Application.DTOs;

public class CreateLinkedTaskRelationDto
{
    public int DependesOnTaskId{get;set;}
    public int LinkedTaskOrder{get;set;}
}
using System;

namespace GestorTareas.Application.DTOs;

public class UserResponseDto
{
    public int Id{get;set;}
    public string UserName { get; set; }
    public string UserLastName { get; set;}
    public string UserEmail { get; set;}
    public bool IsActive { get; set; } = true;
    public bool IsAdmin { get; set; }= false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int UserActiveId {get;set;}
    public DateTime? UpdatedAt { get; set; } = null;
}

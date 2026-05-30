using System;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Application.DTOs;

public class CreateUserDto
{
    [Required(ErrorMessage ="El nombre de usuario es obligatorio.")]
    [MaxLength(30,ErrorMessage = "Longitud máxima de 30 caracteres.")]
    public string UserName { get; set; }
    [Required(ErrorMessage ="El apellido de usuario es obligatorio.")]
    [MaxLength(30,ErrorMessage = "Longitud máxima de 30 caracteres.")]
    public string UserLastName { get; set;}
    [Required(ErrorMessage ="El email de usuario es obligatorio.")]
    [EmailAddress]
    public string UserEmail { get; set;}
    public bool IsActive { get; set; } = true;
    public bool IsAdmin { get; set; }= false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int UserActiveId {get;set;}
    public DateTime? UpdatedAt { get; set; } = null;
}

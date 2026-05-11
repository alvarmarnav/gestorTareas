using System;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Application.DTOs;

public class RegistrationDto
{
     [Required(ErrorMessage ="El nombre de usuario es obligatorio.")]
    [MaxLength(50,ErrorMessage = "Longitud máxima de 50 caracteres.")]
    public string UserName{get;set;} = string.Empty;
    [Required(ErrorMessage ="El apellido de usuario es obligatorio.")]
    [MaxLength(50,ErrorMessage = "Longitud máxima de 50 caracteres.")]
    public string UserLastName { get; set;}=string.Empty;
    [Required,EmailAddress(ErrorMessage ="Email con formato incorrecto.")]
    public string UserEmail{get;set;} = string.Empty;
    [Required,MinLength(8,ErrorMessage ="Longitud mínima de 8 caracteres.")]
    public string UserPassword{get;set;}=string.Empty;
    // public bool IsActive { get; set; } = true;
    // public bool IsAdmin { get; set; }= false;
    // public DateTime CreatedAt { get; set; } = DateTime.Now;
    // public DateTime? UpdatedAt { get; set; } = null;
}

using System;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Application.DTOs;

public class LoginDto
{
    [Required,EmailAddress(ErrorMessage ="El email de usuario es obligatorio")]
    public string UserEmail{get;set;}=string.Empty;
    [Required]
    public string UserPassword{get;set;}=string.Empty;
}

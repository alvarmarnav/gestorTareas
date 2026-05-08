using System;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Application.DTOs;

public class RegistrationDto
{
    [Required(ErrorMessage ="El nombre de usuario es obligatorio.")]
    public string UserName{get;set;} = string.Empty;
    [Required,EmailAddress(ErrorMessage ="Email con formato incorrecto.")]
    public string UserEmail{get;set;} = string.Empty;
    [Required,MinLength(8,ErrorMessage ="Longitud mínima de 8 caracteres.")]
    public string UserPassword{get;set;}=string.Empty;
}

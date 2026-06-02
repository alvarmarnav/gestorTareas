using System;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Application.DTOs;

public class UpdateUserDto
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
    

public UpdateUserDto(
    string userName,
    string userLastName,
    string userEmail
)
    {
        this.UserName=userName;
        this.UserLastName = userLastName;
        this.UserEmail = userEmail;
    }
}

using System;
using GestorTareas.Application.Services;

namespace GestorTareas.Models;

public class User
{
    public Guid Id { get; set; }

    public String UserName
    {
        get; set
        {
            Validator.ValidateString(value, "Nombre Usuario");

            if (value.Length > 20 || value.Length < 3)
                throw new ArgumentOutOfRangeException("La longitud del nombre de usuario no es válida.");

            field = value.Trim();
        }
    }

    public String UserLastName
    {
        get; set
        {
            Validator.ValidateString(value, "Apellidos del usuario");

            if (value.Length > 30 || value.Length < 3)
                throw new ArgumentOutOfRangeException("La longitud de los apellidos no es válida.");

            field = value.Trim();
        }
    }

    public String UserEmail
    {
        get; set
        {
            field = Validator.ValidateEmail(value);
        }
    }

    public Boolean IsActive { get; set; } = true;

    public Boolean IsAdmin { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; } = null;
    public List<Task> tasksList { get; set; } = new(10);
    public User(
        String userName,
        String userLastName,
        String userEmail,
        Boolean isActive,
        Boolean isAdmin
    )
    {
        this.Id = Guid.NewGuid();
        this.UserName = userName;
        this.UserLastName = userLastName;
        this.UserEmail = userEmail;
        this.IsActive = isActive;
        IsAdmin = isAdmin;
        this.CreatedAt = DateTime.Now;
        this.UpdatedAt = null;
    }
}

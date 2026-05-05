using System;
using GestorTareas.Application.Services;

namespace GestorTareas.Models;

public class User
{
    public Guid Id { get; set; }

    public string UserName
    {
        get; set
        {
            Validator.ValidateString(value, "Nombre Usuario");

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El valor está vacío o sólo contiene espacios en blanco.");
            if (value.Length > 20 || value.Length < 3)
                throw new ArgumentOutOfRangeException("La longitud del nombre de usuario no es válida.");

            field = value.Trim();
        }
    }

    public string UserLastName
    {
        get; set
        {
            Validator.ValidateString(value, "Apellidos del usuario");

            if (value.Length > 30 || value.Length < 3)
                throw new ArgumentOutOfRangeException("La longitud de los apellidos no es válida.");

            field = value.Trim();
        }
    }

    public string UserEmail
    {
        get; set
        {
            field = Validator.ValidateEmail(value);
        }
    }

    public bool? IsActive { get; set; } = true;

    public bool? IsAdmin { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; } = null;
    public List<Task> tasksList { get; set; } = new(10);
    
    protected User() : base() { }

    protected User(
        string userName,
        string userLastName,
        string userEmail,
        bool isActive = true,
        bool isAdmin = false
    )
    {
        this.Id = Guid.NewGuid();
        this.UserName = userName.Trim();
        this.UserLastName = userLastName.Trim();
        this.UserEmail = userEmail;
        this.IsActive = isActive;
        IsAdmin = isAdmin;
        this.CreatedAt = DateTime.Now;
        this.UpdatedAt = null;
    }
}

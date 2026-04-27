using System;

namespace GestorTareas.Models;

public class User
{
    public Guid Id { get; set; }

    public String UserName
    {
        get; set
        {
            Validator.ValidateString(value, "Nombre Usuario");

            if (value.Length > 20 || value.Length<3)
                throw new ArgumentOutOfRangeException("La longitud del nombre de usuario no es válida.");

            field = value.Trim();
        }
    }

    public String UserLastName
    {
        get; set
        {
            Validator.ValidateString(value, "Apellidos del usuario");

            if (value.Length > 30 || value.Length<3)
                throw new ArgumentOutOfRangeException("La longitud de los apellidos no es válida.");

            field = value.Trim();
        }
    }

    public String Email
    {
        get; set
        {
            field = Validator.ValidateEmail(value);
        }
    }

    public Boolean UserActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; } = null;

    public User(
        String userName,
        String userLastName,
        String email,
        Boolean userActive
    )
    {
        this.Id = new Guid();
        this.UserName = userName;
        this.UserLastName = userLastName;
        this.Email = email;
        this.UserActive = userActive;
        this.CreatedAt = DateTime.Now;
        this.UpdatedAt = null;
    }
}

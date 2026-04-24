using System;

namespace GestorTareas.Models;

public class User
{
    public Guid Id{get;set;}

    public String UserName {get;set;}

    public String UserLastName {get;set;}

    public String Email {get;set;}

    public Boolean UserActive {get;set;} = true;

    public DateTime CreatedAt{get;set;} = DateTime.Now;

    public DateTime? UpdatedAt {get; set;} = null;

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

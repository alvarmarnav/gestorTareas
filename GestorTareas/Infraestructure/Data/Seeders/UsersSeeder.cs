using System;
using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Models;
using Microsoft.Extensions.Configuration;

namespace GestorTareas.Infraestructure.Data.Seeders;

public class UsersSeeder
{
    private readonly GestorTareasContext _context;
    private readonly IConfiguration _config;

    public UsersSeeder(GestorTareasContext context,IConfiguration configuration) {
         _context = context;
         _config = configuration;
    }
    public async System.Threading.Tasks.Task AsyncSeeder()
    {
       await SeedAdmin();
    }
    public async System.Threading.Tasks.Task SeedAdmin()
    {
        var existsAdminUser = _context.Users.Any(u => u.UserEmail == "admin@mail.com");
        if (!existsAdminUser)
        {
           var userAdmin = new User(){
               UserName = _config["AdminUser:UserName"],
               UserLastName = _config["AdminUser:UserLastName"],
               UserEmail = _config["AdminUser:UserEmail"],
               // Guarda SIEMPRE contraseña hasheada
               PasswordHash = BCrypt.Net.BCrypt.HashPassword(_config["AdminUser:PasswordHash"]),
            };
                    _context.Users.Add(userAdmin);
                    _context.SaveChanges();
        }
    }

}

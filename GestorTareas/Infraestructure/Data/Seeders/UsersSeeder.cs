using GestorTareas.Infraestructure.Repositories;
using GestorTareas.Models;
using Microsoft.EntityFrameworkCore;

public class UsersSeeder
{
    private readonly GestorTareasContext _context;
    private readonly IConfiguration _config;

    public UsersSeeder(GestorTareasContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async System.Threading.Tasks.Task SeedAsync()
    {
        if (await _context.Users.AnyAsync())
            return;

        var admin = new User
        {
            UserName = _config["AdminUser:UserName"],
            UserLastName = _config["AdminUser:UserLastName"],
            UserEmail = _config["AdminUser:UserEmail"],
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                _config["AdminUser:PasswordHash"]
            )
        };

        await _context.Users.AddAsync(admin);
        await _context.SaveChangesAsync();
    }
}
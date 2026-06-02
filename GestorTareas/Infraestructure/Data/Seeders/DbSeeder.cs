using GestorTareas.Models;
using Microsoft.EntityFrameworkCore;

namespace GestorTareas.Infraestructure.Data.Seeders;
public class DbSeeder
{
    private readonly UsersSeeder _usersSeeder;
    private readonly TaskSeeder _taskSeeder;

    public DbSeeder(UsersSeeder usersSeeder, TaskSeeder taskSeeder)
    {
        _usersSeeder = usersSeeder;
        _taskSeeder = taskSeeder;
    }

    public async System.Threading.Tasks.Task SeedAsync()
    {
        await _usersSeeder.SeedAsync();
    }
}
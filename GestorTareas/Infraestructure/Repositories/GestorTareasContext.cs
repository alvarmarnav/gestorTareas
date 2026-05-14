using System;
using GestorTareas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Task = GestorTareas.Models.Task;
using User = GestorTareas.Models.User;

namespace GestorTareas.Infraestructure.Repositories;

public class GestorTareasContext : DbContext
{

    public DbSet<User> Users { get; set; }

    public DbSet<Task> Tasks { get; set; }

    public DbSet<SimpleTask> SimpleTasks { get; set; }

    public DbSet<CompositeTask> CompositeTasks { get; set; }

    public DbSet<SubTask> SubTasks { get; set; }

    public DbSet<LinkedTask> LinkedTasks { get; set; }

    public DbSet<RecurringTask> RecurringTasks { get; set; }

    public GestorTareasContext(DbContextOptions<GestorTareasContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // => optionsBuilder
        // .UseAsyncSeeding(async(context, _, CancellationToken)=>
        // {
        //     var existsUserAdmin = await context.Set<User>().AnyAsync(u => u.UserName =="admin");

        //     if (!existsUserAdmin)
        //     {
        //         context.Set<User>().Add(new User());
        //         await context.SaveChangesAsync();
        //     }
        // }
        // );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Esto aplica todas las configuraciones que encuentre en el proyecto automáticamente
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GestorTareasContext).Assembly);
    }



}

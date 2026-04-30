using System;
using GestorTareas.Models;
using Microsoft.EntityFrameworkCore;
using Task = GestorTareas.Models.Task;

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

    //public DbSet<Task> Tasks{get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // base.OnConfiguring(optionsBuilder);

        // Indicar a EF Core qué proveedor usar y cómo conectarse
        // optionsBuilder.UseSqlServer(
        // @"Server=localhost\;" +
        // "Database=GestorTareas;" +
        // "Trusted_Connection=True;" +
        // "TrustServerCertificate=True;"
        // );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Permitir Patron TPT
        // modelBuilder.Entity<User>().ToTable("Users");
        // modelBuilder.Entity<Task>().ToTable("Tasks");
        // modelBuilder.Entity<SimpleTask>().ToTable("SimpleTasks");
        // modelBuilder.Entity<CompositeTask>().ToTable("CompositeTasks");
        // modelBuilder.Entity<SubTask>().ToTable("SubTasks");
        // modelBuilder.Entity<LinkedTask>().ToTable("LinkedTasks");
        // modelBuilder.Entity<RecurringTask>().ToTable("RecurringTasks");
        
        // Esto aplica todas las configuraciones que encuentre en el proyecto automáticamente
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(GestorTareasContext).Assembly);

       

        


    }
}

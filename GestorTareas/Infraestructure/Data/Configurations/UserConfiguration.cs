using System;
using GestorTareas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

         builder.Property(u => u.UserName)
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(u=> u.UserLastName)
        .HasMaxLength(80)
        .IsRequired();

        builder.HasIndex(u=>u.UserEmail)
        .IsUnique();

        builder.Property(u=>u.IsActive)
        .HasDefaultValue(true)
        .IsRequired();

        builder.Property(u=>u.IsAdmin)
        .HasDefaultValue(false)
        .IsRequired();

        builder.Property(u=>u.CreatedAt)
        .HasDefaultValueSql("GETDATE()")
        .IsRequired();

        builder.Property(u=>u.UpdatedAt)
        .HasDefaultValue(null);

        builder.HasMany(u => u.tasksList)
       .WithMany(t => t.UsersList)
       .UsingEntity(j => j.ToTable("UserTasks")); // Nombre de la tabla intermedia

    }
}

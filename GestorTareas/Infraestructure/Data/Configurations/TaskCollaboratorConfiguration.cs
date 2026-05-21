using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = GestorTareas.Models;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class TaskCollaboratorConfiguration : IEntityTypeConfiguration<Models.TaskCollaborator>
{
    public void Configure(EntityTypeBuilder<Models.TaskCollaborator> builder)
    {
        builder.ToTable("TaskCollaborators");

        // Clave compuesta
        builder.HasKey(tc => new { tc.TaskId, tc.UserId });

        // Relación con la tarea colaborativa
        builder.HasOne(tc => tc.Task)
               .WithMany(t => t.TaskCollaborators)
               .HasForeignKey(tc => tc.TaskId)
               .OnDelete(DeleteBehavior.NoAction);

        // Relación con el usuario
        builder.HasOne(tc => tc.UserTask)
               .WithMany()
               .HasForeignKey(tc => tc.UserId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
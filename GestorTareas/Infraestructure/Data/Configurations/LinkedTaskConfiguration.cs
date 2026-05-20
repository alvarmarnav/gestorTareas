using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class LinkedTaskConfiguration : IEntityTypeConfiguration<Models.LinkedTask>
{
public void Configure(EntityTypeBuilder<Models.LinkedTask> builder)
    {
        builder.ToTable("LinkedTasks");
        
        builder.HasKey(lt => lt.Id);

        builder.Property(lt => lt.LinkedTaskOrder)
            .IsRequired();

        builder.HasOne(lt => lt.Task)
            .WithMany(t => t.Dependencies)
            .HasForeignKey(lt => lt.TaskId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lt => lt.DependsOn)
            .WithMany(t => t.RequiredByOtherTask)
            .HasForeignKey(lt => lt.DependsOnTaskId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

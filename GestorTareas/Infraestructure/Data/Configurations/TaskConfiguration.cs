using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = GestorTareas.Models.Task;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<GestorTareas.Models.Task>
{
    public void Configure(EntityTypeBuilder<GestorTareas.Models.Task> builder)
    {

        builder.ToTable("Tasks");

        builder.Property(t=>t.Title)
        .HasMaxLength(30)
        .IsRequired();
        builder.Property(t=>t.TaskDescription)
        .HasMaxLength(30)
        .HasDefaultValue(null);
        builder.Property(t=>t.Priority)
        .HasDefaultValue(GestorTareas.Enums.TaskPriority.Normal);
        builder.Property(t=>t.Status)
        .HasDefaultValue(GestorTareas.Enums.TaskStatus.Pending);
        builder.Property(t=>t.CreatedAt)
        .HasDefaultValueSql("GETDATE()")
        .IsRequired();
        builder.Property(t=>t.UpdatedAt)
        .HasDefaultValue(null);
        builder.Property(t=>t.DueTime)
        .HasDefaultValue(null);
        builder.Property(t=>t.CancelReason)
        .HasMaxLength(400)
        .HasDefaultValue(null);
        builder.HasOne(t => t.User)
        .WithMany()
        .HasForeignKey("UserId");
    
    }
}

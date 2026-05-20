using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = GestorTareas.Models.Task;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {

        builder.ToTable("Tasks");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
        .ValueGeneratedOnAdd();
        builder.Property(t => t.Title)
        .HasMaxLength(30)
        .IsRequired();
        builder.Property(t => t.TaskDescription)
        .HasMaxLength(30)
        .HasDefaultValue(null);
        builder.Property(t => t.Priority)
        .HasDefaultValue(Enums.TaskPriority.Normal);
        builder.Property(t => t.Status)
        .HasDefaultValue(Enums.TaskStatus.Pending);
        builder.Property(t => t.CreatedAt)
        .HasDefaultValueSql("GETDATE()")
        .IsRequired();
        builder.Property(t => t.UpdatedAt)
        .HasDefaultValue(null);
        builder.Property(t => t.DueTime)
        .HasDefaultValue(null);
        builder.Property(t => t.CancelReason)
        .HasMaxLength(400)
        .HasDefaultValue(null);
        builder.HasOne(t => t.User)
        .WithMany()
        .HasForeignKey("UserId");
    }
}

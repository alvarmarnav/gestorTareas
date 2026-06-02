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
        .HasMaxLength(300)
        .HasDefaultValue(null);
        builder.Property(t => t.TaskType)
        .IsRequired()
    .HasConversion<int>();
        builder.Property(t => t.TaskPriority)
        .IsRequired()
    .HasConversion<int>();
        builder.Property(t => t.TaskStatus)
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
            .WithMany(u => u.TasksList)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

    }
}

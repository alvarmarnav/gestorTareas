using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class CompositeTaskConfiguration
{
    public void Configure(EntityTypeBuilder<GestorTareas.Models.CompositeTask> builder)
    {
        builder.Property(ct => ct.CompositeTaskType)
        .IsRequired();
        builder.Property(ct => ct.LinkedTaskList)
        .IsRequired();
        builder.Property(ct => ct.SubTaskList)
        .IsRequired();
        builder.HasOne(ct => ct.user)
              .WithMany()
              .HasForeignKey("UserId");

    }
}

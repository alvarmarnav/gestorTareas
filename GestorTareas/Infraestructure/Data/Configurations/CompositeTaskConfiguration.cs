using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class CompositeTaskConfiguration : IEntityTypeConfiguration<GestorTareas.Models.CompositeTask>
{
    public void Configure(EntityTypeBuilder<GestorTareas.Models.CompositeTask> builder)
    {
        builder.ToTable("CompositeTasks");

        builder.Property(ct => ct.CompositeTaskType)
        .IsRequired();
        // builder.Property(ct => ct.LinkedTaskList)
        // .IsRequired();
        // builder.Property(ct => ct.SubTaskList)
        // .IsRequired();
        // builder.HasOne(ct => ct.user)
        //       .WithMany()
        //       .HasForeignKey("UserId");
        builder.HasMany(ct => ct.LinkedTaskList)
          .WithOne()
          .HasForeignKey("FKCompositeTaskId_Linked");

        builder.HasMany(ct => ct.SubTaskList)
               .WithOne()
               .HasForeignKey("FKCompositeTaskId_Sub");
    }
}

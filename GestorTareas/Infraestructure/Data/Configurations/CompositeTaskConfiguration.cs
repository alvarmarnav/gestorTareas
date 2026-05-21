using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class CompositeTaskConfiguration : IEntityTypeConfiguration<Models.CompositeTask>
{
  public void Configure(EntityTypeBuilder<Models.CompositeTask> builder)
  {
    builder.ToTable("CompositeTasks");

    // builder.HasMany(ct => ct.SubTaskList)
    // .WithOne()
    // .HasForeignKey("FKCompositeTaskId_Sub");
  }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class LinkedTaskConfiguration : IEntityTypeConfiguration<Models.LinkedTask>
{
public void Configure(EntityTypeBuilder<Models.LinkedTask> builder)
    {
        builder.ToTable("LinkedTasks");
        builder.Property(lt=>lt.LinkedTaskOrder)
        .IsRequired();
        // builder.Property(lt=>lt.ListOfLinkedTasks)
        // .IsRequired();
    }
}

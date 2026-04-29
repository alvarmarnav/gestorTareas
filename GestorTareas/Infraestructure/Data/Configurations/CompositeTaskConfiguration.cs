using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class CompositeTaskConfiguration
{
public void Configure(EntityTypeBuilder<GestorTareas.Models.CompositeTask> builder)
    {
        builder.Property(ct=>ct.CompositeTaskType)
        .IsRequired();

    }
}

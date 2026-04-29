using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class SubTaskConfiguration
{
public void Configure(EntityTypeBuilder<GestorTareas.Models.LinkedTask> builder)
    {
        builder.Property(lt=>lt.LinkedTaskOrder)
        .IsRequired();
        builder.Property(lt=>lt.ListOfLinkedTasks)
        .IsRequired();
    }
}

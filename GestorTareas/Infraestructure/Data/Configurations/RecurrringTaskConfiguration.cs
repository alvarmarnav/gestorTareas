using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class RecurrringTaskConfiguration
{
public void Configure(EntityTypeBuilder<Models.RecurringTask> builder)
    {
        builder.Property(rt=>rt.RecurrenceRule)
        .IsRequired();

        builder.Property(rt=>rt.RecurringTasksCount)
        .HasDefaultValue(0)
        .IsRequired();
    }

}

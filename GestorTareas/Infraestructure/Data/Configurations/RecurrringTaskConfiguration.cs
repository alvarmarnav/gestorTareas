using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class RecurrringTaskConfiguration : IEntityTypeConfiguration<GestorTareas.Models.RecurringTask>
{
public void Configure(EntityTypeBuilder<Models.RecurringTask> builder)
    {
        builder.ToTable("RecurringTasks");

        builder.Property(rt=>rt.RecurrenceRule)
        .IsRequired();

        builder.Property(rt=>rt.RecurringTasksCount)
        .HasDefaultValue(0)
        .IsRequired();
    }

}

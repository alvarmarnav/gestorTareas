using System;
using GestorTareas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas.Infraestructure.Data;

public class SubTaskConfiguration : IEntityTypeConfiguration<GestorTareas.Models.SubTask>
{
    public void Configure(EntityTypeBuilder<SubTask> builder)
    {
        builder.ToTable("SubTasks");
    }
}

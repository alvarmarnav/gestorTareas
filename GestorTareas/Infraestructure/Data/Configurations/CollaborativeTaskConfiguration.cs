using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas;

public class CollaborativeTaskConfiguration : IEntityTypeConfiguration<Models.CollaborativeTask>
{

    public void Configure(EntityTypeBuilder<Models.CollaborativeTask> builder)
    {
        builder.ToTable("CollaborativeTasks");
        
    }
}

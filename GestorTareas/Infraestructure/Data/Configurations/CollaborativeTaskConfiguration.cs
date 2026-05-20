using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas;

public class CollaborativeTaskConfiguration : IEntityTypeConfiguration<Models.CollaborativeTask>
{

public void Configure(EntityTypeBuilder<Models.CollaborativeTask> builder)
    {
        builder.ToTable("CollaborativeTasks");
        // builder.Property(ct => ct.TeamMembers)
        // .IsRequired();
        builder.HasOne(ct => ct.TaskSupervisor)
        .WithMany()
        .HasForeignKey("TaskSupervisorId")
        .OnDelete(DeleteBehavior.SetNull)//TODO: AQUI NO SÉ QUÉ ES MEJOR, PREGUNTAR FRAN
        .IsRequired();
        builder.HasMany(ct => ct.TaskCollaborators)
        .WithMany()
        .UsingEntity(j => j.ToTable("CollaborativeTaskTeamMembers"));
    }
}

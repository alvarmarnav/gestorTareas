using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class SubTaskConfiguration : IEntityTypeConfiguration<Models.SubTask>
{
    public void Configure(EntityTypeBuilder<Models.SubTask> builder)
    {
        builder.ToTable("SubTask");

        builder.HasOne(s => s.ParentCompositeTask)
        .WithMany(c => c.SubTaskList)
        .HasForeignKey(s => s.ParentCompositeTaskId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorTareas.Infraestructure.Data.Configurations;

public class SubTaskConfiguration : IEntityTypeConfiguration<Models.SubTask>
{
    public void Configure(EntityTypeBuilder<Models.SubTask> builder)
    {
        builder.ToTable("SubTask");

        builder.HasKey(st => st.Id);

        builder.Property(st => st.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(st => st.TaskDescription)
            .HasMaxLength(500);

        builder.Property(st => st.Status)
            .IsRequired();

        builder.HasOne(st => st.CompositeTaskFather)
            .WithMany(ct => ct.SubTaskList)
            .HasForeignKey(st => st.CompositeTaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
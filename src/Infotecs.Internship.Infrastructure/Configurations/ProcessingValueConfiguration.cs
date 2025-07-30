using Infotecs.Internship.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infotecs.Internship.Infrastructure.Configurations;

public class ProcessingValueConfiguration : IEntityTypeConfiguration<ProcessingValue>
{
    public void Configure(EntityTypeBuilder<ProcessingValue> builder)
    {
        builder.ToTable("values");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder.HasOne(x => x.ParentFile)
            .WithMany(x => x.Values)
            .HasForeignKey(x => x.ParentFileId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.ParentFileId)
            .HasColumnName("parent_file_id")
            .IsRequired();
        
        builder.Property(x => x.StartedAt)
            .HasColumnName("started_at")
            .IsRequired();
        
        builder.Property(x => x.ExecutionDurationSeconds)
            .HasColumnName("execution_duration_seconds")
            .IsRequired();
        
        builder.Property(x => x.Value)
            .HasColumnName("value")
            .IsRequired();
    }
}
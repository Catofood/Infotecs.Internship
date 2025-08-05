using Infotecs.Internship.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infotecs.Internship.Infrastructure.Configurations;

public class OperationsResultConfiguration : IEntityTypeConfiguration<OperationsResult>
{
    public void Configure(EntityTypeBuilder<OperationsResult> builder)
    {
        builder.ToTable("results");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder.Property(x => x.ParentFileId)
            .HasColumnName("parent_file_id")
            .IsRequired();
        builder.HasOne(x => x.ParentFile)
            .WithOne(x => x.Result)
            .HasForeignKey<OperationsResult>(x => x.ParentFileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(x => x.DateDeltaSeconds)
            .HasColumnName("date_delta_seconds")
            .IsRequired();
        
        builder.Property(x => x.EarliestStartDate)
            .HasColumnName("earliest_start_date")
            .IsRequired();
        builder.HasIndex(x => x.EarliestStartDate);  // Для опционального фильтра поиска
        
        builder.Property(x => x.AverageDurationTimeSeconds)
            .HasColumnName("avg_execution_duration_seconds")
            .IsRequired();
        builder.HasIndex(x => x.AverageDurationTimeSeconds);  // Для опционального фильтра поиска
        
        builder.Property(x => x.AverageValue)
            .HasColumnName("avg_value")
            .IsRequired();
        builder.HasIndex(x => x.AverageValue); // Для опционального фильтра поиска
        
        builder.Property(x => x.MedianValue)
            .HasColumnName("median_value")
            .IsRequired();
        
        builder.Property(x => x.MaxValue)
            .HasColumnName("max_value")
            .IsRequired();
        
        builder.Property(x => x.MinValue)
            .HasColumnName("min_value")
            .IsRequired();
    }
}
using Infotecs.Internship.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infotecs.Internship.Infrastructure.Configurations;

public class ProcessingFileConfiguration : IEntityTypeConfiguration<ProcessingFile>
{
    public void Configure(EntityTypeBuilder<ProcessingFile> builder)
    {
        builder.ToTable("files");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();
    }
}
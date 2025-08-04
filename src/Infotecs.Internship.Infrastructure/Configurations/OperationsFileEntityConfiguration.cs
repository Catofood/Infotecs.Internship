using Infotecs.Internship.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infotecs.Internship.Infrastructure.Configurations;

public class OperationsFileEntityConfiguration : IEntityTypeConfiguration<OperationsFile>
{
    public void Configure(EntityTypeBuilder<OperationsFile> builder)
    {
        builder.ToTable("files");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();
        builder.HasIndex(x => x.Name) // Для опционального фильтра поиска
            .IsUnique(); // Чтобы нельзя было хранить несколько файлов с одним и тем же именем (по тз)
    }
}
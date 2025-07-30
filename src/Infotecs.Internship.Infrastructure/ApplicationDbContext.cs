using System.Reflection;
using Infotecs.Internship.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Internship.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ProcessingFile> ProcessingFiles { get; set; }
    public DbSet<ProcessingResult> ProcessingResults { get; set; }
    public DbSet<ProcessingValue> ProcessingValues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
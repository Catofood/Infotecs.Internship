using System.Reflection;
using Infotecs.Internship.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Internship.Infrastructure;

public class OperationsFileDbContext(DbContextOptions<OperationsFileDbContext> options) : DbContext(options)
{
    public DbSet<OperationsFile> ProcessingFiles { get; set; }
    public DbSet<OperationsResult> ProcessingResults { get; set; }
    public DbSet<OperationValue> ProcessingValues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
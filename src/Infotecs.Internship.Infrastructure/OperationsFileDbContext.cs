using System.Reflection;
using Infotecs.Internship.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Internship.Infrastructure;

public class OperationsFileDbContext(DbContextOptions<OperationsFileDbContext> options) : DbContext(options)
{
    public DbSet<OperationsFile> OperationsFiles { get; set; }
    public DbSet<OperationsResult> OperationsResults { get; set; }
    public DbSet<OperationValue> OperationValues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
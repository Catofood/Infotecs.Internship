using Infotecs.Internship.Application.Contracts;
using Infotecs.Internship.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Internship.Infrastructure.Services;

public class FileRepository : IFileRepository
{
    private readonly OperationsFileDbContext _context;

    public FileRepository(OperationsFileDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<long>> GetIdsByNameAsync(string fileName, CancellationToken cancellationToken)
    {
        var entityIds = await _context.ProcessingFiles
            .Where(f => EF.Functions.ILike(f.Name, fileName))
            .Select(f => f.Id)
            .ToListAsync(cancellationToken);
        return entityIds;
    }

    public async Task CascadeDeleteByIdsAsync(List<long> fileId, CancellationToken cancellationToken)
    {
        await _context.ProcessingFiles
            .Where(f => fileId.Contains(f.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task AddFileAsync(OperationsFile fileEntity, CancellationToken cancellationToken = default)
    {
        await _context.ProcessingFiles.AddAsync(fileEntity, cancellationToken);
    }
}
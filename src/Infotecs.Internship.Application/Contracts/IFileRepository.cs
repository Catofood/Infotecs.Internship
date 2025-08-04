using Infotecs.Internship.Domain.Entities;

namespace Infotecs.Internship.Application.Contracts;

public interface IFileRepository
{
    public Task<List<long>> GetIdsByNameAsync(string fileName, CancellationToken cancellationToken = default);
    public Task CascadeDeleteByIdsAsync(List<long> fileIds, CancellationToken cancellationToken = default);
    public Task AddFileAsync(OperationsFile fileEntity, CancellationToken cancellationToken = default);
}
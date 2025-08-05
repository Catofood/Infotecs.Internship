using Infotecs.Internship.Application.Handlers.GetResultsQuery;
using Infotecs.Internship.Application.Handlers.GetValuesQuery;
using Infotecs.Internship.Application.Pagination;
using Infotecs.Internship.Domain.Entities;

namespace Infotecs.Internship.Application.Contracts;

public interface IFileRepository
{
    public Task<PaginatedList<OperationValuesWithFileName>> GetPagedValuesByQueryAsync(GetValuesQuery searchQuery,
        CancellationToken cancellationToken);
    public Task<PaginatedList<OperationsResultWithFileName>> GetPagedResultsByQueryAsync(GetResultsQuery searchQuery,
        CancellationToken cancellationToken);
    public Task<List<long>> GetIdsByNameAsync(string fileName, CancellationToken cancellationToken = default);
    public Task CascadeDeleteByIdsAsync(List<long> fileIds, CancellationToken cancellationToken = default);
    public Task AddFileAsync(OperationsFile fileEntity, CancellationToken cancellationToken = default);
}
using Infotecs.Internship.Application.Contracts;
using Infotecs.Internship.Application.Pagination;
using MediatR;

namespace Infotecs.Internship.Application.Handlers.GetResultsQuery;

public class GetResultsQueryHandler : IRequestHandler<GetResultsQuery, PaginatedList<OperationsResultWithFileName>>
{
    private readonly IFileRepository _fileRepository;

    public GetResultsQueryHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<PaginatedList<OperationsResultWithFileName>> Handle(GetResultsQuery query, CancellationToken cancellationToken)
    {
        return await _fileRepository.GetPagedResultsByQueryAsync(query, cancellationToken); 
    }
}
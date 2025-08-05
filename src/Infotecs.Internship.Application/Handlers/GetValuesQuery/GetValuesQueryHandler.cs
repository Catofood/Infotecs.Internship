using Infotecs.Internship.Application.Contracts;
using Infotecs.Internship.Application.Pagination;
using Infotecs.Internship.Domain.Entities;
using MediatR;

namespace Infotecs.Internship.Application.Handlers.GetValuesQuery;

public class GetValuesQueryHandler : IRequestHandler<GetValuesQuery, PaginatedList<OperationValuesWithFileName>>
{
    private readonly IFileRepository _fileRepository;

    public GetValuesQueryHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<PaginatedList<OperationValuesWithFileName>> Handle(GetValuesQuery query, CancellationToken cancellationToken)
    {
        return await _fileRepository.GetPagedValuesByQueryAsync(query, cancellationToken);
    }
}
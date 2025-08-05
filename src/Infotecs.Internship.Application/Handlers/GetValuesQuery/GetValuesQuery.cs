using Infotecs.Internship.Application.Pagination;
using Infotecs.Internship.Domain.Entities;
using MediatR;

namespace Infotecs.Internship.Application.Handlers.GetValuesQuery;

public record GetValuesQuery : PaginatedRequest, IRequest<PaginatedList<OperationValuesWithFileName>>
{
    public required string FileName { get; init; }
}
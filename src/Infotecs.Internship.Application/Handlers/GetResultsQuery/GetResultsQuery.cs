using Infotecs.Internship.Application.Pagination;
using MediatR;

namespace Infotecs.Internship.Application.Handlers.GetResultsQuery;

public record GetResultsQuery : PaginatedRequest, IRequest<PaginatedList<OperationsResultWithFileName>>
{
    public string? FileName { get; init; }
    
    public DateTimeOffset? FromStartDate { get; init; }
    public DateTimeOffset? ToStartDate { get; init; }
    
    public double? FromAverageValue { get; init; }
    public double? ToAverageValue { get; init; }
    
    public double? FromAverageDurationTimeSeconds { get; init; }
    public double? ToAverageDurationTimeSeconds { get; init; }
    
}
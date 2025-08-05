using Infotecs.Internship.Application.Pagination;
using Infotecs.Internship.Domain.Entities;

namespace Infotecs.Internship.Application.Handlers.GetValuesQuery;

public record OperationValuesWithFileName
{
    public required string FileName { get; init; }
    public required List<OperationValue> Values { get; init; }
}
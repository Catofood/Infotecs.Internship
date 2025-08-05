using Infotecs.Internship.Domain.Entities;

namespace Infotecs.Internship.Api.Dto;

public class OperationValuesWithFileNameDto
{
    public required string FileName { get; init; }
    public required List<OperationValueDto> PaginatedValues { get; init; }
}
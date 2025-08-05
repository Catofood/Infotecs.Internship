namespace Infotecs.Internship.Api.Dto;

public record OperationValueDto
{
    public required DateTimeOffset StartedAt { get; init; }
    public required long ExecutionDurationSeconds { get; init; }
    public required double Value { get; init; }
}
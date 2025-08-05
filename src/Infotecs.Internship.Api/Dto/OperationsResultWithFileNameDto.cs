namespace Infotecs.Internship.Api.Dto;

public record OperationsResultWithFileNameDto
{
    public required string FileName { get; init; }
    public required double DateDeltaSeconds { get; init; }
    public required DateTimeOffset EarliestStartDate { get; init; }
    public required double AverageDurationTimeSeconds { get; init; }
    public required double AverageValue { get; init; }
    public required double MedianValue { get; init; }
    public required double MaxValue { get; init; }
    public required double MinValue { get; init; }
}
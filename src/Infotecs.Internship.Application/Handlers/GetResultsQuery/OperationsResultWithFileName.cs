namespace Infotecs.Internship.Application.Handlers.GetResultsQuery;

public record OperationsResultWithFileName
{
    public required string FileName { get; set; }
    public required double DateDeltaSeconds { get; set; }
    public required DateTimeOffset EarliestStartDate { get; set; }
    public required double AverageDurationTimeSeconds { get; set; }
    public required double AverageValue { get; set; }
    public required double MedianValue { get; set; }
    public required double MaxValue { get; set; }
    public required double MinValue { get; set; }
}
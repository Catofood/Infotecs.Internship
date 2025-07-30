namespace Infotecs.Internship.Domain.Entities;

public class ProcessingResult
{
    public long Id { get; private set; }
    public required long DateDeltaSeconds { get; set; }
    public required DateTimeOffset EarliestStartDate { get; set; }
    public required long AverageExecutionTimeSeconds { get; set; }
    public required double AverageValue { get; set; }
    public required double MedianValue { get; set; }
    public required double MaxValue { get; set; }
    public required double MinValue { get; set; }
}
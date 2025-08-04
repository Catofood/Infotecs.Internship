namespace Infotecs.Internship.Domain.Entities;

public class OperationsResult
{
    public long Id { get; private set; }
    public long ParentFileId { get; private set; }
    public OperationsFile ParentFile { get; set; }
    public required double DateDeltaSeconds { get; set; }
    public required DateTimeOffset EarliestStartDate { get; set; }
    public required double AverageDurationTimeSeconds { get; set; }
    public required double AverageValue { get; set; }
    public required double MedianValue { get; set; }
    public required double MaxValue { get; set; }
    public required double MinValue { get; set; }
}
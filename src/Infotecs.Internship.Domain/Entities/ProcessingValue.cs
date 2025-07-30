namespace Infotecs.Internship.Domain.Entities;

public class ProcessingValue
{
    public long Id { get; private set; }
    public long FileId { get; private set; }
    public required DateTimeOffset StartedAt { get; set; }
    public required long ExecutionTimeSeconds { get; set; }
    public required double Value { get; set; }
}
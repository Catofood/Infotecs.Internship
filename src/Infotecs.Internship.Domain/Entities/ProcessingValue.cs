namespace Infotecs.Internship.Domain.Entities;

public record ProcessingValue
{
    public long Id { get; private set; }
    public required DateTimeOffset StartedAt { get; set; }
    public required long ExecutionTimeSeconds { get; set; }
    public required double Value { get; set; }
}
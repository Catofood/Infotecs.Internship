namespace Infotecs.Internship.Domain.Entities;

public class ProcessingValue
{
    public long Id { get; private set; }
    public required long ParentFileId { get; set; }
    public required ProcessingFile ParentFile { get; set; }
    public required DateTimeOffset StartedAt { get; set; }
    public required long ExecutionDurationSeconds { get; set; }
    public required double Value { get; set; }
}
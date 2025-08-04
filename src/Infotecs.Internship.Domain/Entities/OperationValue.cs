namespace Infotecs.Internship.Domain.Entities;

public class OperationValue
{
    public long Id { get; private set; }
    public long ParentFileId { get; private set; }
    public OperationsFile ParentFile { get; set; }
    public required DateTimeOffset StartedAt { get; set; }
    public required long ExecutionDurationSeconds { get; set; }
    public required double Value { get; set; }
}
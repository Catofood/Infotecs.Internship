namespace Infotecs.Internship.Domain.Entities;

public class ProcessingFile
{
    public long Id { get; private set; }
    public ProcessingResult? Result { get; set; }
    public ICollection<ProcessingValue> Values { get; set; } = [];
    public required string Name { get; set; }
}
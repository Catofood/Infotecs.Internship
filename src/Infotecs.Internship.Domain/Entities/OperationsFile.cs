namespace Infotecs.Internship.Domain.Entities;

public class OperationsFile
{
    public long Id { get; private set; }
    public OperationsResult Result { get; set; }
    public List<OperationValue> Values { get; set; } = [];
    public required string Name { get; set; }
}
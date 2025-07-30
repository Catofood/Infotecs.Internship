namespace Infotecs.Internship.Domain.Entities;

public record ProcessingFile
{
    public long Id { get; private set; }
    public required string Name { get; set; }
}
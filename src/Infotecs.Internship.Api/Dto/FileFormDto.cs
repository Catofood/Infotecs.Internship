namespace Infotecs.Internship.Api.Dto;

public record FileFormDto
{
    public required IFormFile File {get; init; }
}
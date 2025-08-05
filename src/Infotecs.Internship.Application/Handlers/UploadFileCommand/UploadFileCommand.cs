using MediatR;

namespace Infotecs.Internship.Application.Handlers.UploadFileCommand;

public record UploadFileCommand : IRequest
{
    public required string FileName { get; init; }
    public required Stream FileContentStream { get; init; }
}
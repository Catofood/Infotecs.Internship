using MediatR;

namespace Infotecs.Internship.Application.Handlers.UploadFile;

public record UploadFileCommand : IRequest
{
    public required string FileName { get; set; }
    public required Stream FileContentStream { get; set; }
}
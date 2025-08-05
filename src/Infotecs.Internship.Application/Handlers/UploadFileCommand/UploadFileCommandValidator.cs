using FluentValidation;

namespace Infotecs.Internship.Application.Handlers.UploadFileCommand;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(x => x.FileContentStream)
            .NotNull()
            .Must(s => s.CanRead && s.Length > 0);
        RuleFor(x => x.FileName)
            .NotNull()
            .NotEmpty();
    }
}
using FluentValidation;
using Infotecs.Internship.Application.Options;
using Infotecs.Internship.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Infotecs.Internship.Application.Services.Csv;

public class OperationValueValidator : AbstractValidator<OperationValue>
{
    public OperationValueValidator(IOptionsSnapshot<ValidationOptions> validationOptions)
    {
        var options = validationOptions.Value;
        RuleFor(x => x.StartedAt)
            .NotEmpty()
            .NotNull()
            .GreaterThanOrEqualTo(options.MinStartDateTimeInclusive);
        When(_ => options.UseCurrentTimeAsMaxStartDateTimeInclusive, () =>
        {
            RuleFor(x => x.StartedAt)
                .LessThanOrEqualTo(DateTimeOffset.UtcNow);
        });
        
        RuleFor(x => x.ExecutionDurationSeconds)
            .NotNull()
            .GreaterThan(options.MinExecutionDurationSecondsExclusive);
        RuleFor(x => x.Value)
            .NotNull()
            .GreaterThan(options.MinValueExclusive);
    }
}
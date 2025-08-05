using FluentValidation;

namespace Infotecs.Internship.Application.Handlers.GetResultsQuery;

public class GetResultsQueryValidator : AbstractValidator<GetResultsQuery>
{
    public GetResultsQueryValidator()
    {
        RuleFor(x => x)
            .Must(x => x.FileName != null || 
                       x.FromAverageValue != null ||
                       x.ToAverageValue != null ||
                       x.FromStartDate != null ||
                       x.ToStartDate != null ||
                       x.FromAverageDurationTimeSeconds != null ||
                       x.ToAverageDurationTimeSeconds != null)
            .WithMessage("This query should contain at least one property.");
    }
}
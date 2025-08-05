namespace Infotecs.Internship.Application.Options;

public class ValidationOptions
{
    public const string ConfigurationSectionName = "ValidationOptions";
    public DateTimeOffset MinStartDateTimeInclusive { get; init; }
    public bool UseCurrentTimeAsMaxStartDateTimeInclusive { get; init; }
    public int MinExecutionDurationSecondsExclusive { get; init; }
    public double MinValueExclusive { get; init; }
    public int MinCsvContentLinesCountInclusive { get; init; }
    public int MaxCsvContentLinesCountInclusive { get; init; }

    public int MaxPageSize { get; init; }
    public int MinPageSize { get; init; }

}
// "ValidationOptions": {
//     "MinStartDateTimeInclusive": "2024-01-01T00:00:00Z",
//     "UseCurrentTimeAsMaxStartDateTimeInclusive": true,
//     "MinExecutionDurationSecondsExclusive": 0,
//     "MinValueExclusive": 0,
//     "MinCsvContentLinesCountInclusive": 1,
//     "MaxCsvContentLinesCountInclusive": 10000
// }
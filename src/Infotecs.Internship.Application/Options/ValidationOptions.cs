namespace Infotecs.Internship.Application.Options;

public class ValidationOptions
{
    public const string ConfigurationSectionName = "ValidationOptions";
    public DateTimeOffset MinStartDateTimeInclusive { get; init; }
    public DateTimeOffset? MaxStartDateTimeInclusive { get; init; }
    public int MinExecutionDurationSecondsInclusive { get; init; }
    public double MinValueInclusive { get; init; }
    public int MinCsvContentLinesCountInclusive { get; init; }
    public int MaxCsvContentLinesCountInclusive { get; init; }

    public int MaxPageSize { get; init; }
    public int MinPageSize { get; init; }

}
// "ValidationOptions": {
//     "MinStartDateTimeInclusive": "2000-01-01T00:00:00Z",
//     "UseCurrentTimeAsMaxStartDateTimeInclusive": true,
//     "MinExecutionDurationSecondsInclusive": 0,
//     "MinValueInclusive": 0.0,
//     "MinCsvContentLinesCountInclusive": 1,
//     "MaxCsvContentLinesCountInclusive": 10000,
//     "MinPageSize": 10,
//     "MaxPageSize": 100
// }
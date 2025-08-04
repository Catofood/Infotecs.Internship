namespace Infotecs.Internship.Application.Options;

public class ValidationOptions
{
    public const string ConfigurationSectionName = "ValidationOptions";
    public DateTimeOffset MinStartDateTimeInclusive { get; set; }
    public bool UseCurrentTimeAsMaxStartDateTimeInclusive { get; set; }
    public int MinExecutionDurationSecondsExclusive { get; set; }
    public double MinValueExclusive { get; set; }
    public int MinCsvContentLinesCountInclusive { get; set; }
    public int MaxCsvContentLinesCountInclusive { get; set; }
    
}
// "ValidationOptions": {
//     "MinStartDateTimeInclusive": "2024-01-01T00:00:00Z",
//     "UseCurrentTimeAsMaxStartDateTimeInclusive": true,
//     "MinExecutionDurationSecondsExclusive": 0,
//     "MinValueExclusive": 0,
//     "MinCsvContentLinesCountInclusive": 1,
//     "MaxCsvContentLinesCountInclusive": 10000
// }
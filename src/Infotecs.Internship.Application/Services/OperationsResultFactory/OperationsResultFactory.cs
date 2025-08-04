using Infotecs.Internship.Domain.Entities;
using LinqStatistics;

namespace Infotecs.Internship.Application.Services.OperationsResultFactory;

public class OperationsResultFactory : IOperationsResultFactory
{
    public OperationsResult Create(List<OperationValue> operationValues)
    {
        var result = new OperationsResult()
        {
            AverageValue = operationValues.Average(x => x.Value),
            MedianValue = operationValues.Median(x => x.Value),
            MaxValue = operationValues.Max(x => x.Value),
            MinValue = operationValues.Min(x => x.Value),
            AverageDurationTimeSeconds = operationValues.Average(x => x.ExecutionDurationSeconds),
            EarliestStartDate = operationValues.Min(x => x.StartedAt),
            DateDeltaSeconds = (operationValues.Max(x => x.StartedAt) - operationValues.Min(x => x.StartedAt))
                .TotalSeconds
        };
        return result;
    }
}
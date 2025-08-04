using CsvHelper.Configuration;
using Infotecs.Internship.Domain.Entities;

namespace Infotecs.Internship.Application.Services.Csv;

public sealed class OperationValueMap : ClassMap<OperationValue>
{
    public OperationValueMap()
    {
        Map(m => m.StartedAt).Name("Date");
        Map(m => m.ExecutionDurationSeconds).Name("ExecutionTime");
        Map(m => m.Value).Name("Value");
        Map(m => m.Id).Ignore();
        Map(m => m.ParentFileId).Ignore();
        Map(m => m.ParentFile).Ignore();
    }

}
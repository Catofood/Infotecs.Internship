using Infotecs.Internship.Domain.Entities;

namespace Infotecs.Internship.Application.Services.Csv;

public interface IOperationsFileCsvService
{
    Task<List<OperationValue>> ParseAndValidateCsv(Stream csvStream,
        CancellationToken cancellationToken);
}
using Infotecs.Internship.Domain.Entities;

namespace Infotecs.Internship.Application.Services.OperationsResultFactory;

public interface IOperationsResultFactory
{
    OperationsResult Create(List<OperationValue> operationValues);
}
using FluentValidation;
using Infotecs.Internship.Application.Contracts;
using Infotecs.Internship.Application.Services.Csv;
using Infotecs.Internship.Application.Services.OperationsResultFactory;
using Infotecs.Internship.Domain.Entities;
using LinqStatistics;
using MediatR;

namespace Infotecs.Internship.Application.Handlers.UploadFile;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand>
{
    private readonly IValidator<OperationValue> _validator;
    private readonly IOperationsFileCsvService _fileCsvService;
    private readonly IFileRepository _fileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOperationsResultFactory _resultFactory;

    public UploadFileCommandHandler(IValidator<OperationValue> validator, IOperationsFileCsvService fileCsvService, IFileRepository fileRepository, IUnitOfWork unitOfWork, IOperationsResultFactory resultFactory)
    {
        _validator = validator;
        _fileCsvService = fileCsvService;
        _fileRepository = fileRepository;
        _unitOfWork = unitOfWork;
        _resultFactory = resultFactory;
    }

    public async Task Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var operationValues = await _fileCsvService.ParseAndValidateCsv(request.FileContentStream, cancellationToken);
        
        var fileName = request.FileName.ToLower();

        var valueEntities = operationValues.Select(x => new OperationValue() { 
            ExecutionDurationSeconds = x.ExecutionDurationSeconds,
            Value = x.Value,
            StartedAt = x.StartedAt}).ToList();

        var result = _resultFactory.Create(valueEntities);

        var file = new OperationsFile()
        {
            Name = fileName,
            Values = valueEntities,
            Result = result
        };
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var oldFileIds = await _fileRepository.GetIdsByNameAsync(fileName, cancellationToken);
            await _fileRepository.CascadeDeleteByIdsAsync(oldFileIds, cancellationToken);
            await _fileRepository.AddFileAsync(file, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

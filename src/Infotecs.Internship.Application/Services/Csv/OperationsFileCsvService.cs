using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using FluentValidation;
using Infotecs.Internship.Application.Options;
using Infotecs.Internship.Domain.Entities;
using Microsoft.Extensions.Options;
using ValidationException = FluentValidation.ValidationException;

namespace Infotecs.Internship.Application.Services.Csv;

public class OperationsFileCsvService : IOperationsFileCsvService
{
    private readonly IValidator<OperationValue> _validator;
    private readonly ValidationOptions _options;

    public OperationsFileCsvService(IValidator<OperationValue> validator, IOptionsSnapshot<ValidationOptions> validationRules)
    {
        _validator = validator;
        _options = validationRules.Value;
    }

    public async Task<List<OperationValue>> ParseAndValidateCsv(Stream csvStream,
        CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            IgnoreBlankLines = false,
            DetectColumnCountChanges = true,
            Delimiter = ";",
        });
        csv.Context.RegisterClassMap<OperationValueMap>();
        
        var processedOperationValues = new List<OperationValue>();
        await foreach (var value in csv.GetRecordsAsync<OperationValue>(cancellationToken)
                           .WithCancellation(cancellationToken))
        {
            if (processedOperationValues.Count > _options.MaxCsvContentLinesCountInclusive) 
                throw new ValidationException(
                    $"File contains more values than maximum: {_options.MaxCsvContentLinesCountInclusive}.");
            await _validator.ValidateAndThrowAsync(value, cancellationToken);
            processedOperationValues.Add(value);
        }
        if (processedOperationValues.Count < _options.MinCsvContentLinesCountInclusive) 
            throw new ValidationException(
                $"File contains more values than minimum: {_options.MinCsvContentLinesCountInclusive}.");
        return processedOperationValues;
    }
}
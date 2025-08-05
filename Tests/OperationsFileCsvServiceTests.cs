using System.Globalization;
using System.Text;
using CsvHelper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Infotecs.Internship.Application.Options;
using Infotecs.Internship.Application.Services.Csv;
using Infotecs.Internship.Domain.Entities;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;


namespace Tests;

public class OperationsFileCsvServiceTests
{
    private readonly Mock<IValidator<OperationValue>> _validatorMock;
    private readonly ValidationOptions _validationOptions;
    
    public OperationsFileCsvServiceTests()
    {
        _validatorMock = new Mock<IValidator<OperationValue>>();
        _validatorMock
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<OperationValue>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _validationOptions = new ValidationOptions()
        {
            MinCsvContentLinesCountInclusive = 1,
            MaxCsvContentLinesCountInclusive = 10, // Тестовое значение чтобы не прогонять 10000 раз
        };
    }

    private Stream GenerateCsv(params string[] lines)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Date;ExecutionTime;Value");
        foreach (var line in lines)
        {
            sb.AppendLine(line);
        }
        return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
    }

    [Fact]
    public async Task ParseAndValidate_ShouldReturn_Valid_OperationValues()
    {
        var dt = DateTimeOffset.UtcNow.AddHours(-1);
        const int executionSeconds = 10;
        const double value = 11.1;
        // Arrange
        var line = string.Format(CultureInfo.InvariantCulture,
            "{0:yyyy-MM-ddTHH:mm:ss.ffffZ};{1};{2}",
            dt, executionSeconds, value);
        var csvStream = GenerateCsv(line);
        
        var optionsSnapshot = Mock.Of<IOptionsSnapshot<ValidationOptions>>(o =>
            o.Value == _validationOptions);
        var service = new OperationsFileCsvService(_validatorMock.Object, optionsSnapshot);
        
        // Act
        var result = await service.ParseAndValidateCsv(csvStream, CancellationToken.None);
        
        // Assert
        result.Should().HaveCount(1);
        result[0].StartedAt.Should().BeCloseTo(dt, TimeSpan.FromMilliseconds(1));
        result[0].ExecutionDurationSeconds.Should().Be(executionSeconds);
        result[0].Value.Should().Be(value);
        _validatorMock.Verify(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    // [Fact]
    
}
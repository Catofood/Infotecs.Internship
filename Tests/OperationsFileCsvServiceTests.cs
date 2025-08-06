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
using ValidationException = FluentValidation.ValidationException;

namespace Tests;

public class OperationsFileCsvServiceTests
{
    private readonly Mock<IValidator<OperationValue>> _validatorMock;
    private readonly ValidationOptions _validationOptions;

    public OperationsFileCsvServiceTests()
    {
        _validatorMock = new Mock<IValidator<OperationValue>>();
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<OperationValue>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _validationOptions = new ValidationOptions
        {
            MinCsvContentLinesCountInclusive = 1,
            MaxCsvContentLinesCountInclusive = 10
        };
    }

    private OperationsFileCsvService CreateService()
    {
        var optionsSnapshot = Mock.Of<IOptionsSnapshot<ValidationOptions>>(o => o.Value == _validationOptions);
        return new OperationsFileCsvService(_validatorMock.Object, optionsSnapshot);
    }

    private Stream GenerateCsv(params string[] lines)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Date;ExecutionTime;Value");
        foreach (var line in lines)
            sb.AppendLine(line);
        return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
    }

    private static Stream GenerateCsvWithLines(int lineCount)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Date;ExecutionTime;Value");
        for (int i = 0; i < lineCount; i++)
        {
            // Форматируем секунды с ведущим нулём
            var seconds = i.ToString("D2");
            sb.AppendLine($"2024-01-01T12:00:{seconds}Z;10;1.1");
        }
        return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
    }


    public static IEnumerable<object[]> GetInvalidCsvLines()
    {
        yield return ["2025-13-99T99:99:99.9999Z;10;11.1"];    // Неверная дата
        yield return [";10;11.1"];                             // Пустая дата
        yield return ["2025-08-05T20:00:00.0000Z;abc;11.1"];   // ExecutionTime не число
        yield return ["2025-08-05T20:00:00.0000Z;;11.1"];      // Пустой ExecutionTime
        yield return ["2025-08-05T20:00:00.0000Z;10;xyz"];     // Value не число
        yield return ["2025-08-05T20:00:00.0000Z;10;"];        // Пустой Value
        yield return ["2025-08-05T20:00:00.0000Z;10"];         // Не хватает колонок
        yield return ["2025-08-05T20:00:00.0000Z;10;11.1;EXTRA"]; // Лишняя колонка
    }

    [Fact]
    public async Task ParseAndValidate_ShouldReturn_Valid_OperationValues()
    {
        // Arrange
        var dt = DateTimeOffset.UtcNow.AddHours(-1);
        const int executionSeconds = 10;
        const double value = 11.1;
        var line = string.Format(CultureInfo.InvariantCulture,
            "{0:yyyy-MM-ddTHH:mm:ss.ffffZ};{1};{2}", dt, executionSeconds, value);
        var csvStream = GenerateCsv(line);
        var service = CreateService();

        // Act
        var result = await service.ParseAndValidateCsv(csvStream, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result[0].StartedAt.Should().BeCloseTo(dt, TimeSpan.FromMilliseconds(1));
        result[0].ExecutionDurationSeconds.Should().Be(executionSeconds);
        result[0].Value.Should().Be(value);
        _validatorMock.Verify(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [MemberData(nameof(GetInvalidCsvLines))]
    public async Task ParseAndValidate_ShouldThrow_CsvHelperException_WhenCsvLineIsInvalid(string invalidLine)
    {
        var csvStream = GenerateCsv(invalidLine);
        var service = CreateService();

        var ex = await Record.ExceptionAsync(() => service.ParseAndValidateCsv(csvStream, CancellationToken.None));
        ex.Should().BeAssignableTo<CsvHelperException>();
    }

    [Fact]
    public async Task Should_Pass_When_LineCount_Equals_Min()
    {
        var service = CreateService();
        await using var stream = GenerateCsvWithLines(_validationOptions.MinCsvContentLinesCountInclusive);
        var result = await service.ParseAndValidateCsv(stream, CancellationToken.None);
        result.Should().HaveCount(_validationOptions.MinCsvContentLinesCountInclusive);
    }

    [Fact]
    public async Task Should_Pass_When_LineCount_Equals_Max()
    {
        var service = CreateService();
        await using var stream = GenerateCsvWithLines(_validationOptions.MaxCsvContentLinesCountInclusive);
        var result = await service.ParseAndValidateCsv(stream, CancellationToken.None);
        result.Should().HaveCount(_validationOptions.MaxCsvContentLinesCountInclusive);
    }

    [Fact]
    public async Task Should_Throw_When_LineCount_Less_Than_Min()
    {
        var service = CreateService();
        var linesCount = _validationOptions.MinCsvContentLinesCountInclusive - 1;
        if (linesCount < 0) linesCount = 0;

        await using var stream = GenerateCsvWithLines(linesCount);

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.ParseAndValidateCsv(stream, CancellationToken.None));
    }


    [Fact]
    public async Task Should_Throw_When_LineCount_More_Than_Max()
    {
        var service = CreateService();
        await using var stream = GenerateCsvWithLines(_validationOptions.MaxCsvContentLinesCountInclusive + 2);

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.ParseAndValidateCsv(stream, CancellationToken.None));
    }


    [Fact]
    public async Task Should_Pass_When_LineCount_Within_Limits()
    {
        var service = CreateService();
        await using var stream = GenerateCsvWithLines(3);
        var result = await service.ParseAndValidateCsv(stream, CancellationToken.None);
        result.Should().HaveCount(3);
    }
}

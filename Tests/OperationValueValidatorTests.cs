using FluentValidation;
using FluentValidation.TestHelper;
using Infotecs.Internship.Application.Handlers.UploadFileCommand;
using Infotecs.Internship.Application.Options;
using Infotecs.Internship.Application.Services.Csv;
using Infotecs.Internship.Domain.Entities;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests;

public class OperationValueValidatorTests
{
    private readonly OperationValueValidator _validator;

    public OperationValueValidatorTests()
    {
        var options = new ValidationOptions
        {
            MinStartDateTimeInclusive = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
            UseCurrentTimeAsMaxStartDateTimeInclusive = true,
            MinExecutionDurationSecondsInclusive = 0,
        };
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ValidationOptions>>();
        optionsSnapshotMock.Setup(x => x.Value).Returns(options);
        _validator = new OperationValueValidator(optionsSnapshotMock.Object);
    }

    [Fact]
    public void Should_Have_Error_When_DateTime_Is_Before_2000_01_01()
    {
        // Arrange
        var entity = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(-1),
            Value = 1,
        };

        // Act
        var result = _validator.TestValidate(entity);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StartedAt);
    }
    
    [Fact]
    public void Should_Have_Error_When_DateTime_Is_Later_Than_Current_DateTime()
    {
        // Arrange
        var entity = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = DateTimeOffset.UtcNow.AddMilliseconds(0),
            Value = 1,
        };
        // Act
        var result = _validator.TestValidate(entity);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StartedAt);
    }

    [Fact]
    public void Should_Have_Error_When_Value_Is_Less_Than_Zero()
    {
        // Arrange
        var entity = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = DateTimeOffset.UtcNow.AddHours(-1),
            Value = -double.Epsilon,
        };
        
        // Act
        var result = _validator.TestValidate(entity);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Value);
    }

    [Fact]
    public void Should_Have_Error_When_ExecutionDuration_Is_Less_Than_Zero()
    {
        // Arrange
        var entity = new OperationValue()
        {
            ExecutionDurationSeconds = -1,
            StartedAt = DateTimeOffset.UtcNow.AddHours(-1),
            Value = 1,
        };
        
        // Act
        var result = _validator.TestValidate(entity);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ExecutionDurationSeconds);
    }
}
using FluentValidation.TestHelper;
using Infotecs.Internship.Application.Options;
using Infotecs.Internship.Application.Services.Csv;
using Infotecs.Internship.Domain.Entities;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests;

public class OperationValueValidatorTests
{
    private readonly OperationValueValidator _validator;

    private readonly ValidationOptions _validationOptions;

    public OperationValueValidatorTests()
    {
        var options = new ValidationOptions
        {
            MinStartDateTimeInclusive = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
            MaxStartDateTimeInclusive = null,
            MinValueInclusive = 0,
            MinExecutionDurationSecondsInclusive = 0,
        };
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<ValidationOptions>>();
        optionsSnapshotMock.Setup(x => x.Value).Returns(options);
        _validator = new OperationValueValidator(optionsSnapshotMock.Object);
        _validationOptions = optionsSnapshotMock.Object.Value;
    }

    [Fact]
    public void StartedAt_ShouldBeMoreThanOrEqualTo_MinStartDateTimeInclusive()
    {
        var entityOutOfBoundary = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = _validationOptions.MinStartDateTimeInclusive.AddSeconds(-1),
            Value = 1,
        };
        var resultOutOfBoundary = _validator.TestValidate(entityOutOfBoundary);
        resultOutOfBoundary.ShouldHaveValidationErrorFor(x => x.StartedAt);

        var entityBoundary = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = _validationOptions.MinStartDateTimeInclusive,
            Value = 1,
        };
        var resultBoundary = _validator.TestValidate(entityBoundary);
        resultBoundary.ShouldNotHaveValidationErrorFor(x => x.StartedAt);

        var entityInsideBoundaries = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = _validationOptions.MinStartDateTimeInclusive.AddSeconds(1),
            Value = 1,
        };
        var resultInsideBoundaries = _validator.TestValidate(entityInsideBoundaries);
        resultInsideBoundaries.ShouldNotHaveValidationErrorFor(x => x.StartedAt);
    }

    [Fact]
    public void StartedAt_ShouldBeLessThanOrEqualTo_MaxStartDateTimeInclusive()
    {
        var maxStartDateTimeInclusive = _validationOptions.MaxStartDateTimeInclusive ?? DateTimeOffset.UtcNow;
        var entityOutOfBoundaries = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = maxStartDateTimeInclusive.AddSeconds(1),
            Value = 1,
        };
        var resultOutOfBoundaries = _validator.TestValidate(entityOutOfBoundaries);
        resultOutOfBoundaries.ShouldHaveValidationErrorFor(x => x.StartedAt);
        
        var entityBoundary = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = maxStartDateTimeInclusive.AddSeconds(0),
            Value = 1,
        };
        var resultBoundary = _validator.TestValidate(entityBoundary);
        resultBoundary.ShouldNotHaveValidationErrorFor(x => x.StartedAt);
        
        var entityInsideBoundaries = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = maxStartDateTimeInclusive.AddSeconds(-1),
            Value = 1,
        };
        var resultInsideBoundaries = _validator.TestValidate(entityInsideBoundaries);
        resultInsideBoundaries.ShouldNotHaveValidationErrorFor(x => x.StartedAt);
    }

    [Fact]
    public void Value_ShouldBeGreaterThanOrEqualTo_MinValueInclusive()
    {
        var minValueInclusive = _validationOptions.MinValueInclusive;
        // TODO: Использовать переменную
        var entityOutOfBoundary = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = DateTimeOffset.UtcNow.AddHours(-1),
            Value = -double.Epsilon+minValueInclusive,
        };
        var resultOutOfBoundary = _validator.TestValidate(entityOutOfBoundary);
        resultOutOfBoundary.ShouldHaveValidationErrorFor(x => x.Value);

        var entityBoundary = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = DateTimeOffset.UtcNow.AddHours(-1),
            Value = minValueInclusive,
        };
        var resultBoundary = _validator.TestValidate(entityBoundary);
        resultBoundary.ShouldNotHaveValidationErrorFor(x => x.Value);
        
        var entityInsideBoundaries = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = DateTimeOffset.UtcNow.AddHours(-1),
            Value = double.Epsilon+minValueInclusive,
        };
        var resultInsideBoundaries = _validator.TestValidate(entityInsideBoundaries);
        resultInsideBoundaries.ShouldNotHaveValidationErrorFor(x => x.Value);
    }
    
    [Fact]
    public void ExecutionDuration_ShouldBeGreaterThanOrEqualTo_MinExecutionDurationSecondsInclusive()
    {
        var minExecutionDurationSecondsInclusive = _validationOptions.MinExecutionDurationSecondsInclusive;
        // Arrange
        var entityOutOfBoundary = new OperationValue()
        {
            ExecutionDurationSeconds = minExecutionDurationSecondsInclusive-1,
            StartedAt = DateTimeOffset.UtcNow.AddHours(-1),
            Value = 1,
        };
        var resultOutOfBoundary = _validator.TestValidate(entityOutOfBoundary);
        resultOutOfBoundary.ShouldHaveValidationErrorFor(x => x.ExecutionDurationSeconds);
        
        var entityBoundary = new OperationValue()
        {
            ExecutionDurationSeconds = minExecutionDurationSecondsInclusive,
            StartedAt = DateTimeOffset.UtcNow.AddHours(-1),
            Value = 1,
        };
        var resultBoundary = _validator.TestValidate(entityBoundary);
        resultBoundary.ShouldNotHaveValidationErrorFor(x => x.ExecutionDurationSeconds);
        
        var entityInsideBoundaries = new OperationValue()
        {
            ExecutionDurationSeconds = minExecutionDurationSecondsInclusive+1,
            StartedAt = DateTimeOffset.UtcNow.AddHours(-1),
            Value = 1,
        };
        var resultInsideBoundaries = _validator.TestValidate(entityInsideBoundaries);
        resultInsideBoundaries.ShouldNotHaveValidationErrorFor(x => x.ExecutionDurationSeconds);
    }
}
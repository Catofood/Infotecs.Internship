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
        var badEntity = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = _validationOptions.MinStartDateTimeInclusive.AddSeconds(-1),
            Value = 1,
        };
        
        var okEntity1 = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = _validationOptions.MinStartDateTimeInclusive,
            Value = 1,
        };
        
        var okEntity2 = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = _validationOptions.MinStartDateTimeInclusive.AddSeconds(1),
            Value = 1,
        };

        
        var badResult = _validator.TestValidate(badEntity);
        badResult.ShouldHaveValidationErrorFor(x => x.StartedAt);
        
        var okResult1 = _validator.TestValidate(okEntity1);
        okResult1.ShouldNotHaveValidationErrorFor(x => x.StartedAt);
        
        var okResult2 = _validator.TestValidate(okEntity2);
        okResult2.ShouldNotHaveValidationErrorFor(x => x.StartedAt);
    }
    
    [Fact]
    public void StartedAt_ShouldBeLessThanOrEqualTo_MaxStartDateTimeInclusive()
    {
        var maxStartDateTimeInclusive = _validationOptions.MaxStartDateTimeInclusive ?? DateTimeOffset.UtcNow;
        var badEntity = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = maxStartDateTimeInclusive.AddSeconds(1),
            Value = 1,
        };
        var result = _validator.TestValidate(badEntity);
        result.ShouldHaveValidationErrorFor(x => x.StartedAt);
        
        var okEntity1 = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = maxStartDateTimeInclusive.AddMilliseconds(1),
            Value = 1,
        };
        var okResult1 = _validator.TestValidate(okEntity1);
        okResult1.ShouldNotHaveValidationErrorFor(x => x.StartedAt);
        
        var okEntity2 = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = maxStartDateTimeInclusive.AddMilliseconds(1),
            Value = 1,
        };
        var okResult2 = _validator.TestValidate(okEntity2);
        okResult2.ShouldNotHaveValidationErrorFor(x => x.StartedAt);
    }

    [Fact]
    public void Value_ShouldBeGreaterThanOrEqualTo_MinValueInclusive()
    {
        var entity = new OperationValue()
        {
            ExecutionDurationSeconds = 1,
            StartedAt = DateTimeOffset.UtcNow.AddHours(-1),
            Value = -double.Epsilon,
        };
        
        var result = _validator.TestValidate(entity);
        result.ShouldHaveValidationErrorFor(x => x.Value);
    }
    // TODO: Улучшить тесты
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
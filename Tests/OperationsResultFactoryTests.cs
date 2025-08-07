using FluentAssertions;
using Infotecs.Internship.Application.Services.OperationsResultFactory;
using Infotecs.Internship.Domain.Entities;

namespace Tests;

public class OperationsResultFactoryTests
{
    private readonly OperationsResultFactory _factory = new();

    [Fact]
    public void Create_Should_Calculate_Correct_Aggregates()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var values = new List<OperationValue>
        {
            new() { Value = 10, ExecutionDurationSeconds = 2, StartedAt = now },
            new() { Value = 20, ExecutionDurationSeconds = 4, StartedAt = now.AddSeconds(10) },
            new() { Value = 30, ExecutionDurationSeconds = 6, StartedAt = now.AddSeconds(20) }
        };

        // Act
        var result = _factory.Create(values);

        // Assert
        result.AverageValue.Should().Be(20);
        result.MedianValue.Should().Be(20);
        result.MaxValue.Should().Be(30);
        result.MinValue.Should().Be(10);
        result.AverageDurationTimeSeconds.Should().Be(4);
        result.EarliestStartDate.Should().Be(now);
        result.DateDeltaSeconds.Should().Be(20);
    }

    [Fact]
    public void Create_Should_Work_With_Single_Value()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var values = new List<OperationValue>
        {
            new() { Value = 42, ExecutionDurationSeconds = 5, StartedAt = now }
        };

        // Act
        var result = _factory.Create(values);

        // Assert
        result.AverageValue.Should().Be(42);
        result.MedianValue.Should().Be(42);
        result.MaxValue.Should().Be(42);
        result.MinValue.Should().Be(42);
        result.AverageDurationTimeSeconds.Should().Be(5);
        result.EarliestStartDate.Should().Be(now);
        result.DateDeltaSeconds.Should().Be(0);
    }

    [Fact]
    public void Create_Should_Throw_On_Empty_List()
    {
        // Arrange
        var values = new List<OperationValue>();

        // Act
        var act = () => _factory.Create(values);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}
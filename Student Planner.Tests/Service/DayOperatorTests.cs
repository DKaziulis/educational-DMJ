using Xunit;
using Moq;
using Student_Planner.Models;
using Student_Planner.Repositories.Interfaces;
using Student_Planner.Services.Implementations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class DayOperatorTests
{
    private readonly Mock<IDayRepository> _dayRepositoryMock;
    private readonly DayOperator _dayOperator;

    public DayOperatorTests()
    {
        _dayRepositoryMock = new Mock<IDayRepository>();
        _dayOperator = new DayOperator(_dayRepositoryMock.Object);
    }

    [Fact]
    public void LoadDays_ShouldLoadAllDaysFromRepository()
    {
        // Arrange
        var days = new List<Day>
        {
            new Day { Date = DateOnly.MinValue },
            new Day { Date = DateOnly.MaxValue }
        };
        _dayRepositoryMock.Setup(repo => repo.GetAll()).Returns(days);

        // Act
        var result = _dayOperator.LoadDays();

        // Assert
        Assert.Equal(days.Count, result.Count);
    }
    
    [Fact]
    public void FindDayForEvent_ShouldReturnNullIfNotExists()
    {
        // Arrange
        var day = new Day { Date = DateOnly.MinValue };
        var days = new ConcurrentDictionary<DateOnly, Day>();
        _dayRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Day> { day });

        // Act
        var result = _dayOperator.FindDayForEvent(DateOnly.MaxValue);

        // Assert
        Assert.Null(result);
    }
}
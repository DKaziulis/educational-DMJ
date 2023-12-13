using Xunit;
using Microsoft.EntityFrameworkCore;
using Student_Planner.Databases;
using Student_Planner.Models;
using Student_Planner.Repositories.Implementations;
using System.Linq;

public class DayRepositoryTests
{
    private readonly DayRepository _dayRepository;
    private readonly EventsDBContext _dbContext;

    public DayRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<EventsDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique name for each database
            .Options;

        _dbContext = new EventsDBContext(options);
        _dayRepository = new DayRepository(_dbContext);
    }


    [Fact]
    public void GetAll_ShouldReturnAllDays()
    {
        // Arrange
        var day1 = new Day { Id = 1, Date = DateOnly.MinValue };
        var day2 = new Day { Id = 2, Date = DateOnly.MaxValue };
        _dbContext.Days.AddRange(day1, day2);
        _dbContext.SaveChanges();

        // Act
        var result = _dayRepository.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetById_ShouldReturnDayIfExists()
    {
        // Arrange
        var day = new Day { Id = 1, Date = DateOnly.MinValue };
        _dbContext.Days.Add(day);
        _dbContext.SaveChanges();

        // Act
        var result = _dayRepository.GetById(day.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(day.Id, result.Id);
    }

    [Fact]
    public void Add_ShouldAddNewDay()
    {
        // Arrange
        var day = new Day { Id = 1, Date = DateOnly.MinValue };

        // Act
        _dayRepository.Add(day);
        _dbContext.SaveChanges();

        // Assert
        var result = _dbContext.Days.Find(day.Id);
        Assert.NotNull(result);
        Assert.Equal(day.Id, result.Id);
    }

    [Fact]
    public void Update_ShouldUpdateExistingDay()
    {
        // Arrange
        var day = new Day { Id = 1, Date = DateOnly.MinValue };
        _dbContext.Days.Add(day);
        _dbContext.SaveChanges();
        day.Date = DateOnly.MaxValue;

        // Act
        _dayRepository.Update(day);
        _dbContext.SaveChanges();

        // Assert
        var result = _dbContext.Days.Find(day.Id);
        Assert.NotNull(result);
        Assert.Equal(day.Date, result.Date);
    }

    [Fact]
    public void Delete_ShouldRemoveExistingDay()
    {
        // Arrange
        var day = new Day { Id = 1, Date = DateOnly.MinValue };
        _dbContext.Days.Add(day);
        _dbContext.SaveChanges();

        // Act
        _dayRepository.Delete(day);
        _dbContext.SaveChanges();

        // Assert
        var result = _dbContext.Days.Find(day.Id);
        Assert.Null(result);
    }

    [Fact]
    public void GetByDate_ShouldReturnDayIfExists()
    {
        // Arrange
        var day = new Day { Id = 1, Date = DateOnly.MinValue };
        _dbContext.Days.Add(day);
        _dbContext.SaveChanges();

        // Act
        var result = _dayRepository.GetByDate(day.Date);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(day.Date, result.Date);
    }

}
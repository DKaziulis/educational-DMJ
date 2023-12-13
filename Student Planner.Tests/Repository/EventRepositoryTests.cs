using Xunit;
using Microsoft.EntityFrameworkCore;
using Student_Planner.Databases;
using Student_Planner.Models;
using Student_Planner.Repositories.Implementations;
using System.Linq;

public class EventRepositoryTests
{
    private readonly EventRepository _eventRepository;
    private readonly EventsDBContext _dbContext;

    public EventRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<EventsDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique name for each database
            .Options;

        _dbContext = new EventsDBContext(options);
        _eventRepository = new EventRepository(_dbContext);
    }

    [Fact]
    public void GetAll_ShouldReturnAllEvents()
    {
        // Arrange
        var event1 = new Event { Id = 1, DayId = 1 };
        var event2 = new Event { Id = 2, DayId = 2 };
        _dbContext.Events.AddRange(event1, event2);
        _dbContext.SaveChanges();

        // Act
        var result = _eventRepository.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetById_ShouldReturnEventIfExists()
    {
        // Arrange
        var _event = new Event { Id = 1, DayId = 1 };
        _dbContext.Events.Add(_event);
        _dbContext.SaveChanges();

        // Act
        var result = _eventRepository.GetById(_event.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_event.Id, result.Id);
    }

    [Fact]
    public void Add_ShouldAddNewEvent()
    {
        // Arrange
        var _event = new Event { Id = 1, DayId = 1 };

        // Act
        _eventRepository.Add(_event);
        _dbContext.SaveChanges();

        // Assert
        var result = _dbContext.Events.Find(_event.Id);
        Assert.NotNull(result);
        Assert.Equal(_event.Id, result.Id);
    }

    [Fact]
    public void Update_ShouldUpdateExistingEvent()
    {
        // Arrange
        var _event = new Event { Id = 1, DayId = 1 };
        _dbContext.Events.Add(_event);
        _dbContext.SaveChanges();
        _event.DayId = 2;

        // Act
        _eventRepository.Update(_event);
        _dbContext.SaveChanges();

        // Assert
        var result = _dbContext.Events.Find(_event.Id);
        Assert.NotNull(result);
        Assert.Equal(_event.DayId, result.DayId);
    }

    [Fact]
    public void Delete_ShouldRemoveExistingEvent()
    {
        // Arrange
        var _event = new Event { Id = 1, DayId = 1 };
        _dbContext.Events.Add(_event);
        _dbContext.SaveChanges();

        // Act
        _eventRepository.Delete(_event);
        _dbContext.SaveChanges();

        // Assert
        var result = _dbContext.Events.Find(_event.Id);
        Assert.Null(result);
    }

    [Fact]
    public void GetByDayId_ShouldReturnEventIfExists()
    {
        // Arrange
        var _event = new Event { Id = 1, DayId = 1 };
        _dbContext.Events.Add(_event);
        _dbContext.SaveChanges();

        // Act
        var result = _eventRepository.GetByDayId(_event.DayId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_event.DayId, result.DayId);
    }

    [Fact]
    public void GetAllByDayId_ShouldReturnAllEventsForDay()
    {
        // Arrange
        var event1 = new Event { Id = 1, DayId = 1 };
        var event2 = new Event { Id = 2, DayId = 1 };
        var event3 = new Event { Id = 3, DayId = 2 };
        _dbContext.Events.AddRange(event1, event2, event3);
        _dbContext.SaveChanges();

        // Act
        var result = _eventRepository.GetAllByDayId(1);

        // Assert
        Assert.Equal(2, result.Count());
    }
}

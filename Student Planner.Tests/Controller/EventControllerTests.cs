using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Student_Planner.Controllers;
using Student_Planner.Repositories.Interfaces;
using Student_Planner.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Student_Planner.Models;

public class EventsControllerTests
{
    private readonly Mock<IDayRepository> _dayRepositoryMock = new();
    private readonly Mock<IEventRepository> _eventRepositoryMock = new();
    private readonly Mock<IEventServices> _eventServicesMock = new();
    private readonly Mock<ILogger<EventsController>> _loggerMock = new();

    [Fact]
    public void Index_ReturnsViewResult_WithListOfDays()
    {
        // Arrange
        var days = new List<Day> { new Day(), new Day() };
        _dayRepositoryMock.Setup(repo => repo.GetAll()).Returns(days);
        var controller = new EventsController(_dayRepositoryMock.Object, _eventRepositoryMock.Object, _eventServicesMock.Object, _loggerMock.Object);

        // Act
        var result = controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<Day>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count);
    }

    [Fact]
    public void Create_RedirectsToIndex_WhenModelStateIsValid()
    {
        // Arrange
        var newEvent = new Event();
        _eventServicesMock.Setup(service => service.CreateEvent(newEvent));
        var controller = new EventsController(_dayRepositoryMock.Object, _eventRepositoryMock.Object, _eventServicesMock.Object, _loggerMock.Object);

        // Act
        var result = controller.Create(newEvent);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Null(redirectToActionResult.ControllerName);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Fact]
    public void Edit_ThrowsArgumentException_WhenDayIsNull()
    {
        // Arrange
        var eventId = 1;
        var dayDate = "2023-12-31";
        _dayRepositoryMock.Setup(repo => repo.GetByDate(DateOnly.Parse(dayDate))).Returns((Day)null);
        var controller = new EventsController(_dayRepositoryMock.Object, _eventRepositoryMock.Object, _eventServicesMock.Object, _loggerMock.Object);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => controller.Edit(eventId, dayDate));
        Assert.Equal("Day not found/Events is null", ex.Message);
    }




    //[Fact]
    //public void Edit_ReturnsViewResult_WithExistingEvent()
    //{
    //    // Arrange
    //    var existingEvent = new Event { Id = 1 };
    //    var dayDate = "2023-12-31";
    //    var existingDay = new Day { Date = DateOnly.Parse(dayDate) };
    //    _dayRepositoryMock.Setup(repo => repo.GetByDate(DateOnly.Parse(dayDate))).Returns(existingDay);
    //    _eventRepositoryMock.Setup(repo => repo.GetById(existingEvent.Id)).Returns(existingEvent);
    //    var controller = new EventsController(_dayRepositoryMock.Object, _eventRepositoryMock.Object, _eventServicesMock.Object, _loggerMock.Object);

    //    // Act
    //    var result = controller.Edit(existingEvent.Id, dayDate);

    //    // Assert
    //    var viewResult = Assert.IsType<ViewResult>(result);
    //    var model = Assert.IsAssignableFrom<Event>(viewResult.ViewData.Model);
    //    Assert.Equal(existingEvent.Id, model.Id);
    //}

    //[Fact]
    //public void EditPost_ReturnsRedirectResult_WhenModelStateIsValid()
    //{
    //    // Arrange
    //    var updatedEvent = new Event { Id = 1 };
    //    var dayDate = DateOnly.Parse("2023-12-31");
    //    var existingDay = new Day { Date = dayDate };
    //    _dayRepositoryMock.Setup(repo => repo.GetByDate(dayDate)).Returns(existingDay);
    //    _eventServicesMock.Setup(service => service.EditEvent(existingDay, updatedEvent));
    //    var controller = new EventsController(_dayRepositoryMock.Object, _eventRepositoryMock.Object, _eventServicesMock.Object, _loggerMock.Object);

    //    // Act
    //    var result = controller.Edit(updatedEvent, dayDate, null);

    //    // Assert
    //    var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
    //    Assert.Null(redirectToActionResult.ControllerName);
    //    Assert.Equal("Index", redirectToActionResult.ActionName);
    //}


    //[Fact]
    //public void Delete_ReturnsViewResult_WithExistingEvent()
    //{
    //    // Arrange
    //    var existingEvent = new Event { Id = 1 };
    //    var dayDate = "2023-12-31";
    //    var existingDay = new Day { Date = DateOnly.Parse(dayDate) };
    //    _dayRepositoryMock.Setup(repo => repo.GetByDate(DateOnly.Parse(dayDate))).Returns(existingDay);
    //    _eventRepositoryMock.Setup(repo => repo.GetById(existingEvent.Id)).Returns(existingEvent);
    //    var controller = new EventsController(_dayRepositoryMock.Object, _eventRepositoryMock.Object, _eventServicesMock.Object, _loggerMock.Object);

    //    // Act
    //    var result = controller.Delete(existingEvent.Id, dayDate);

    //    // Assert
    //    var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
    //    var model = Assert.IsAssignableFrom<Event>(viewResult.ViewData.Model);
    //    Assert.Equal(existingEvent.Id, model.Id);
    //}


    //[Fact]
    //public void DeleteConfirmed_ReturnsRedirectResult_WhenEventExists()
    //{
    //    // Arrange
    //    var existingEvent = new Event { Id = 1 };
    //    var existingDay = new Day { Id = 1 };
    //    _dayRepositoryMock.Setup(repo => repo.GetById(existingDay.Id)).Returns(existingDay);
    //    _eventRepositoryMock.Setup(repo => repo.GetById(existingEvent.Id)).Returns(existingEvent);
    //    _eventServicesMock.Setup(service => service.DeleteEvent(existingEvent, existingDay));
    //    var controller = new EventsController(_dayRepositoryMock.Object, _eventRepositoryMock.Object, _eventServicesMock.Object, _loggerMock.Object);

    //    // Act
    //    var result = controller.DeleteConfirmed(existingEvent.Id, existingDay.Id, null);

    //    // Assert
    //    var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
    //    Assert.Null(redirectToActionResult.ControllerName);
    //    Assert.Equal("Index", redirectToActionResult.ActionName);
    //}

    

    //[Fact]
    //public void Edit_ReturnsBadRequestResult_WhenEventOrDayNotFound()
    //{
    //    // Arrange
    //    var dayRepositoryMock = new Mock<IDayRepository>();
    //    var eventRepositoryMock = new Mock<IEventRepository>();
    //    var eventServicesMock = new Mock<IEventServices>();
    //    var loggerMock = new Mock<ILogger<EventsController>>();

    //    dayRepositoryMock.Setup(repo => repo.GetByDate(It.IsAny<DateOnly>())).Returns((Day)null);
    //    eventRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Event)null);

    //    var controller = new EventsController(dayRepositoryMock.Object, eventRepositoryMock.Object, eventServicesMock.Object, loggerMock.Object);

    //    // Act
    //    var result = controller.Edit(1, "2023-01-01") as BadRequestResult;

    //    // Assert
    //    Assert.NotNull(result);
    //}


}
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Student_Planner.Controllers;
using Student_Planner.DTO_s;
using Student_Planner.Models;
using Student_Planner.Repositories.Interfaces;
using Student_Planner.Services.Interfaces;
using Xunit;

public class EventsApiControllerTests
{
    [Fact]
    public void GetAllEvents_ReturnsOkObjectResult_WithListOfEvents()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockDayRepository = new Mock<IDayRepository>();
        var mockEventServices = new Mock<IEventServices>();

        var controller = new EventsApiController(
            mockDayRepository.Object,
            mockEventRepository.Object,
            mockEventServices.Object
        );

        var sampleEvents = new List<Event>
        {
            new Event { Id = 1, Name = "Event 1" },
            new Event { Id = 2, Name = "Event 2" }
        };

        mockEventRepository.Setup(repo => repo.GetAll()).Returns(sampleEvents);

        // Act
        var result = controller.GetAllEvents();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Event>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var model = Assert.IsAssignableFrom<IEnumerable<Event>>(okResult.Value);

        Assert.NotNull(model);
        Assert.NotEmpty(model);
    }



    [Fact]
    public void AddEvent_WithValidModel_ReturnsCreatedAtAction()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockDayRepository = new Mock<IDayRepository>();
        var mockEventServices = new Mock<IEventServices>();

        var controller = new EventsApiController(
            mockDayRepository.Object,
            mockEventRepository.Object,
            mockEventServices.Object
        );

        var eventDTO = new EventDTO
        {
            Name = "New Event",
            BeginDate = DateTime.Now,
            StartTime = new TimeOnly(10, 0), 
            Description = "Event description"
        };

        // Act
        var result = controller.AddEvent(eventDTO);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdAtActionResult.StatusCode); 
    }

    [Fact]
    public void EditEvent_WithValidModel_ReturnsOkObjectResult_WithUpdatedEvent()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockDayRepository = new Mock<IDayRepository>();
        var mockEventServices = new Mock<IEventServices>();

        var controller = new EventsApiController(
            mockDayRepository.Object,
            mockEventRepository.Object,
            mockEventServices.Object
        );

        var existingEventId = 1; 
        var existingEvent = new Event { Id = existingEventId, Name = "Old Event" };

        mockEventRepository.Setup(repo => repo.GetById(existingEventId)).Returns(existingEvent);

        var eventDTO = new EventDTO
        {
            Name = "Updated Event",
            BeginDate = DateTime.Now,
            StartTime = new TimeOnly(12, 0),
            Description = "Updated description"
        };

        // Act
        var result = controller.EditEvent(existingEventId, eventDTO);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var updatedEvent = Assert.IsType<Event>(okResult.Value);

        Assert.Equal(eventDTO.Name, updatedEvent.Name);
        Assert.Equal(eventDTO.BeginDate, updatedEvent.BeginDate);
        Assert.Equal(eventDTO.StartTime, updatedEvent.StartTime);
        Assert.Equal(eventDTO.Description, updatedEvent.Description);
    }

    [Fact]
    public void DeleteEvent_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var mockEventRepository = new Mock<IEventRepository>();
        var mockDayRepository = new Mock<IDayRepository>();
        var mockEventServices = new Mock<IEventServices>();

        var controller = new EventsApiController(
            mockDayRepository.Object,
            mockEventRepository.Object,
            mockEventServices.Object
        );

        var existingEventId = 1;
        var existingEvent = new Event { Id = existingEventId, Name = "Existing Event" };

        mockEventRepository.Setup(repo => repo.GetById(existingEventId)).Returns(existingEvent);

        // Act
        var result = controller.DeleteEvent(existingEventId);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);

    }



}

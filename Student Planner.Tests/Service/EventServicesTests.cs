using Microsoft.Extensions.Logging;
using Moq;
using Student_Planner.Models;
using Student_Planner.Repositories.Interfaces;
using Student_Planner.Services.Implementations;
using System;
using Student_Planner.Services.Interfaces;

namespace Student_Planner.Tests.Service
{
    public class EventServicesTests
    {
        private readonly Mock<IDayRepository> _dayRepositoryMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<ILogger<EventServices>> _loggerMock;
        private readonly Mock<IDayOperator> _dayOperatorMock;

        public EventServicesTests()
        {
            _dayRepositoryMock = new Mock<IDayRepository>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _loggerMock = new Mock<ILogger<EventServices>>();
            _dayOperatorMock = new Mock<IDayOperator>();
        }

        [Fact]
        public void CreateEvent_ValidEvent_AddsEventToRepository()
        {
            // Arrange
            var eventServices = new EventServices(_dayRepositoryMock.Object, _eventRepositoryMock.Object, _loggerMock.Object, _dayOperatorMock.Object);
            var newEvent = new Event { Name = "Test Event", BeginDate = DateTime.Now };

            // Act
            eventServices.CreateEvent(newEvent);

            // Assert
            _eventRepositoryMock.Verify(r => r.Add(It.Is<Event>(e => e.Name == newEvent.Name)), Times.Once);
            _dayRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void EditEvent_ExistingDayAndEvent_UpdatesEvent()
        {
            // Arrange
            var eventServices = new EventServices(_dayRepositoryMock.Object, _eventRepositoryMock.Object, _loggerMock.Object, _dayOperatorMock.Object);
            var existingDay = new Day();
            var updatedEvent = new Event { Id = 1, Name = "Updated Event", BeginDate = DateTime.Now };

            _eventRepositoryMock.Setup(r => r.GetById(updatedEvent.Id)).Returns(new Event());

            // Act
            eventServices.EditEvent(existingDay, updatedEvent);

            // Assert
            _eventRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteEvent_ExistingEventAndDay_DeletesEventAndUpdatesDay()
        {
            // Arrange
            var eventServices = new EventServices(_dayRepositoryMock.Object, _eventRepositoryMock.Object, _loggerMock.Object, _dayOperatorMock.Object);
            var existingDay = new Day { NumOfEvents = 1 };
            var existingEvent = new Event { DayId = existingDay.Id };

            _eventRepositoryMock.Setup(r => r.GetByDayId(existingDay.Id)).Returns(new Event());

            // Act
            eventServices.DeleteEvent(existingEvent, existingDay);

            // Assert
            _eventRepositoryMock.Verify(r => r.Delete(It.Is<Event>(e => e.DayId == existingDay.Id)), Times.Once);
            Assert.Equal(0, existingDay.NumOfEvents);
            _dayRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        }

    }
}

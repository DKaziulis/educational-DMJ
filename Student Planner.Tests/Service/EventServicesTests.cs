using Moq;
using Student_Planner.Models;
using Student_Planner.Repositories.Interfaces;
using Student_Planner.Services;
using System;

namespace Student_Planner.Tests.Service
{
    public class EventServicesTests
    {
        [Fact]
        public void CreateEvent_ValidEvent_EventIsCreated()
        {
            // Arrange
            var mockDayRepo = new Mock<IDayRepository>();
            var mockEventRepo = new Mock<IEventRepository>();
            var service = new EventServices(mockDayRepo.Object, mockEventRepo.Object);
            var newEvent = new Event();

            // Act
            service.CreateEvent(newEvent);

            // Assert
            mockEventRepo.Verify(repo => repo.Add(newEvent), Times.Once);
            mockDayRepo.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public void EditEvent_ValidEvent_EventIsEdited()
        {
            // Arrange
            var mockDayRepo = new Mock<IDayRepository>();
            var mockEventRepo = new Mock<IEventRepository>();
            var service = new EventServices(mockDayRepo.Object, mockEventRepo.Object);
            var existingDay = new Day();
            var updatedEvent = new Event();

            // Act
            service.EditEvent(existingDay, updatedEvent);

            // Assert
            mockEventRepo.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteEvent_ValidEvent_EventIsDeleted()
        {
            // Arrange
            var mockDayRepo = new Mock<IDayRepository>();
            var mockEventRepo = new Mock<IEventRepository>();
            var service = new EventServices(mockDayRepo.Object, mockEventRepo.Object);
            var existingDay = new Day();
            var existingEvent = new Event();

            // Act
            service.DeleteEvent(existingEvent, existingDay);

            // Assert
            mockEventRepo.Verify(repo => repo.Delete(existingEvent), Times.Once);
            mockDayRepo.Verify(repo => repo.SaveChanges(), Times.Once);
        }
    }
}

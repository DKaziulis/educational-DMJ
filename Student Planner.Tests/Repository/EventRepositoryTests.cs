using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using Student_Planner.Databases;
using Student_Planner.Models;
using Student_Planner.Repositories.Implementations;
using Xunit;

public class EventRepositoryTests
{
    [Fact]
    public void GetAll_ReturnsAllEvents()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EventsDBContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryEventDB")
            .Options;

        using (var dbContext = new EventsDBContext(options))
        {
            var events = new List<Event>
            {
                new Event { Id = 1, Name = "Event 1" },
                new Event { Id = 2, Name = "Event 2" },
                new Event { Id = 3, Name = "Event 3" }
            };

            dbContext.Events.AddRange(events);
            dbContext.SaveChanges();

            var eventRepository = new EventRepository(dbContext);

            // Act
            var result = eventRepository.GetAll();

            // Assert
            Assert.Equal(events.Count, result.Count());
        }
    }

    


}

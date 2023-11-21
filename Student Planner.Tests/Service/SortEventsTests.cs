using Student_Planner.Enums;
using Student_Planner.Models;
using Student_Planner.Services;

namespace Student_Planner.Tests.Service
{
    public class EventExtensionsTests
    {
        [Fact]
        public void SortEvents_SortByName_EventsAreSorted()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event { Name = "B" },
                new Event { Name = "A" },
                new Event { Name = "C" }
            };

            // Act
            var sortedEvents = events.SortEvents(EventSortKey.Name);

            // Assert
            Assert.Equal("A", sortedEvents[0].Name);
            Assert.Equal("B", sortedEvents[1].Name);
            Assert.Equal("C", sortedEvents[2].Name);
        }
    }
}

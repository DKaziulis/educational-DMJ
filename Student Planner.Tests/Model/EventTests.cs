using Xunit;
using Student_Planner.Models;
using System;
using Student_Planner.Controllers;

namespace Student_Planner.Tests.Model
{
    public class EventTests
    {
        [Fact]
        public void Event_Constructor_Sets_Properties_Correctly()
        {
            // Arrange
            string name = "Test Event";
            DateTime beginDate = DateTime.Today;
            TimeOnly eventEndTime = new TimeOnly(10, 0);
            string description = "Test Description";
            TimeDuration.Time eventDuration = new TimeDuration.Time(10, 0, 0);


            // Act
            var eventObj = new Event(name, beginDate, eventEndTime, description, eventDuration);

            // Assert
            Assert.Equal(name, eventObj.Name);
            Assert.Equal(beginDate, eventObj.BeginDate);
            Assert.Equal(eventEndTime, eventObj.EndTime);
            Assert.Equal(description, eventObj.Description);
            Assert.Equal(eventDuration.ToTimeSpan(), eventObj.EventDuration);
        }

        [Fact]
        public void Event_Default_Constructor_Works_Correctly()
        {
            // Act
            var eventObj = new Event();

            // Assert
            Assert.Equal(default(int), eventObj.Id);
            Assert.Null(eventObj.Name);
            Assert.Equal(default(DateTime), eventObj.BeginDate);
            Assert.Equal(default(TimeOnly), eventObj.StartTime);
            Assert.Equal(default(TimeOnly), eventObj.EndTime);
            Assert.Null(eventObj.Description);
            Assert.Equal(default(TimeSpan), eventObj.EventDuration);
            Assert.Equal(default(EventsController.CourseGroup), eventObj.CourseGroup);
        }

        [Fact]
        public void CalculateEventDuration_Returns_Correct_Duration()
        {
            // Arrange
            var eventObj = new Event
            {
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 0)
            };

            // Act
            var duration = eventObj.EventDuration;

            // Assert
            Assert.Equal(new TimeSpan(1, 0, 0), duration);
        }
    }
}

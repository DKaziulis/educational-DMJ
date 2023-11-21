using Xunit;
using System;
using Student_Planner.Models;
using Student_Planner.Controllers;
using Student_Planner.Models.Exceptions;

namespace Student_Planner.Tests.Model
{
    public class EventTests
    {
        [Fact]
        public void Constructor_ValidInputs_ObjectIsInitialized()
        {
            // Arrange
            var name = "Test Event";
            var beginDate = DateTime.Now;
            var endTime = new TimeOnly(10, 0);
            var description = "This is a test event.";
            var eventDuration = new TimeDuration.Time(1, 0,0);

            // Act
            var testEvent = new Event(name, beginDate, endTime, description, eventDuration);

            // Assert
            Assert.Equal(name, testEvent.Name);
            Assert.Equal(beginDate, testEvent.BeginDate);
            Assert.Equal(endTime, testEvent.EndTime);
            Assert.Equal(description, testEvent.Description);
        }

        [Fact]
        public void CalculateEventDuration_ValidTimes_CorrectDuration()
        {
            // Arrange
            var testEvent = new Event();
            testEvent.StartTime = new TimeOnly(9, 0);
            testEvent.EndTime = new TimeOnly(10, 0);

            // Act
            var duration = testEvent.CalculateEventDuration();

            // Assert
            Assert.Equal(new TimeSpan(1, 0, 0), duration);
        }

        [Fact]
        public void Name_SetValidName_NameIsSet()
        {
            // Arrange
            var testEvent = new Event();
            var validName = "Test Event";

            // Act
            testEvent.Name = validName;

            // Assert
            Assert.Equal(validName, testEvent.Name);
        }

        [Fact]
        public void Name_SetInvalidName_ThrowsCharacterException()
        {
            // Arrange
            var testEvent = new Event();
            var invalidName = "Invalid#Name";

            // Act & Assert
            var exception = Assert.Throws<CharacterException>(() => testEvent.Name = invalidName);
            Assert.Equal("Invalid name format.", exception.Message);
        }

        [Fact]
        public void CompareTo_NullEvent_ReturnsOne()
        {
            // Arrange
            var testEvent = new Event();

            // Act
            var result = testEvent.CompareTo(null);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void CompareTo_EarlierEvent_ReturnsPositive()
        {
            // Arrange
            var testEvent1 = new Event();
            testEvent1.BeginDate = DateTime.Now;

            var testEvent2 = new Event();
            testEvent2.BeginDate = DateTime.Now.AddDays(-1);

            // Act
            var result = testEvent1.CompareTo(testEvent2);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void CompareTo_LaterEvent_ReturnsNegative()
        {
            // Arrange
            var testEvent1 = new Event();
            testEvent1.BeginDate = DateTime.Now;

            var testEvent2 = new Event();
            testEvent2.BeginDate = DateTime.Now.AddDays(1);

            // Act
            var result = testEvent1.CompareTo(testEvent2);

            // Assert
            Assert.True(result < 0);
        }
    }
}

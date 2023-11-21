using Student_Planner.Enums;
using Student_Planner.Models;
using System;
using Student_Planner.Services;

namespace Student_Planner.Tests.Service
{
    public class DayExtensionsTests
    {
        [Fact]
        public void SortDays_SortByDate_DaysAreSorted()
        {
            // Arrange
            var days = new List<Day>
            {
                new Day { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) },
                new Day { Date = DateOnly.FromDateTime(DateTime.Now) },
                new Day { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)) }
            };

            // Act
            var sortedDays = days.SortDays(DaySortKey.Date);

            // Assert
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now), sortedDays[0].Date);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), sortedDays[1].Date);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now.AddDays(2)), sortedDays[2].Date);

        }

        [Fact]
        public void SortDays_SortByNumOfEvents_DaysAreSorted()
        {
            // Arrange
            var days = new List<Day>
            {
                new Day { NumOfEvents = 2 },
                new Day { NumOfEvents = 1 },
                new Day { NumOfEvents = 3 }
            };

            // Act
            var sortedDays = days.SortDays(DaySortKey.NumOfEvents);

            // Assert
            Assert.Equal(1, sortedDays[0].NumOfEvents);
            Assert.Equal(2, sortedDays[1].NumOfEvents);
            Assert.Equal(3, sortedDays[2].NumOfEvents);
        }
    }
}

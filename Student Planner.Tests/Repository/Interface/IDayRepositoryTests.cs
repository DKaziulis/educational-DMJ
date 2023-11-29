using Moq;
using Student_Planner.Models;
using Student_Planner.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Student_Planner.Tests.Repository.Interface
{
    public class IDayRepositoryTests
    {
        private readonly Mock<IDayRepository> _mockRepo;

        public IDayRepositoryTests()
        {
            _mockRepo = new Mock<IDayRepository>();
        }

        [Fact]
        public void GetByDate_ReturnsDay()
        {
            // Arrange
            var testDate = new DateOnly(2023, 11, 28);
            var testDay = new Day { Date = testDate };
            _mockRepo.Setup(repo => repo.GetByDate(testDate)).Returns(testDay);

            // Act
            var result = _mockRepo.Object.GetByDate(testDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testDate, result.Date);
        }
    }
}

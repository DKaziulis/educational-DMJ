using Microsoft.Extensions.Logging;
using Moq;
using System.Web.Mvc;
using Xunit;
using Student_Planner.Controllers;
using Student_Planner.Models;
using Student_Planner.Services;


namespace Student_Planner.Tests.Controller
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_mockLogger.Object);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.ViewResult>(result);
        }


        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            // Act
            var result = _controller.Privacy();

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.ViewResult>(result);
        }

    }

}

using Microsoft.AspNetCore.Mvc;
using Student_Planner.Models;
using Student_Planner.Services.Implementations;
using System.Diagnostics;

namespace Student_Planner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string filePath = "ReadFiles/ApplicationDescription.txt";
            string fileContent = ReadTxtFile.ReadTextFile(filePath);

            ViewData["Message"] = fileContent;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
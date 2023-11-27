using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Student_Planner.Models;
using Student_Planner.Services.Implementations;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Student_Planner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            string filePath = "ReadFiles/ApplicationDescription.txt";

            // Use Task.Run to execute the synchronous method asynchronously
            string fileContent = await Task.Run(() => ReadTxtFile.ReadTextFile(filePath));

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
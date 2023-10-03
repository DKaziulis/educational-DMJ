using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Student_Planner.Models;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace Student_Planner.Controllers
{
    public class EventController : Controller
    {
        public IActionResult DayEvent(Event newEvent)
        {
            string jsonString = JsonSerializer.Serialize(newEvent.EventName);
            Console.WriteLine(jsonString);
            var viewModel = newEvent;
            return View(viewModel);
        }
    }
}

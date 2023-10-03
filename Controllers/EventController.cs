using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Student_Planner.Models;
using System.Security.Cryptography.X509Certificates;

namespace Student_Planner.Controllers
{
    public class EventController : Controller
    {
        public IActionResult DayEvent(string eventName)
        {
            ViewBag.Message = eventName;
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Student_Planner.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

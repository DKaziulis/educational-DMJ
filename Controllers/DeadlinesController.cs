using Microsoft.AspNetCore.Mvc;

namespace Student_Planner.Controllers
{
    public class DeadlinesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

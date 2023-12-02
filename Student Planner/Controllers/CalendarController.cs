using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Student_Planner.Repositories.Interfaces;

namespace Student_Planner.Controllers
{
    public class CalendarController : Controller
    {
        private readonly IEventRepository _eventRepository;
        public CalendarController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public IActionResult Index()
        {
            var events = _eventRepository.GetAll();
            return View(events);
        }
    }
}

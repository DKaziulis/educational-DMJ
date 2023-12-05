using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Student_Planner.Repositories.Interfaces;
using Student_Planner.Models;

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

        public IActionResult EventsList(string date)
        {
            int dayId = Convert.ToInt32(date.Replace("-", ""));

            var events = _eventRepository.GetAllByDayId(dayId).ToList();

            ViewBag.Events = events;
            ViewBag.Date = date;

            return View();
        }
    }
}

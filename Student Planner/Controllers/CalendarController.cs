using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Student_Planner.Repositories.Interfaces;
using Student_Planner.Models;
using Student_Planner.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Student_Planner.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventServices _eventServices;
        public CalendarController(IEventRepository eventRepository, IEventServices eventServices)
        {
            _eventRepository = eventRepository;
            _eventServices = eventServices;
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
        public IActionResult Create(string dayDate)
        {
            ViewBag.Date = dayDate;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Event newEvent)
        {
            try
            {
                string? dayDate = Request.Form["dayDate"];

                DateTime dayDateTime = DateTime.Parse(dayDate);

                newEvent.BeginDate = dayDateTime;
                if (ModelState.IsValid)
                {
                    _eventServices.CreateEvent(newEvent);

                    return RedirectToAction("Index");
                }
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
            }
            return View(newEvent);
        }
    }
}

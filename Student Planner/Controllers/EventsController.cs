using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Planner.Models;
using Student_Planner.Enums;
using Student_Planner.Repositories.Interfaces;
using Student_Planner.Services.Implementations;
using Student_Planner.Services.Interfaces;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Student_Planner.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly IDayRepository _dayRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventServices _eventServices;
        private readonly ILogger _logger;
        public EventsController(IDayRepository dayRepository, IEventRepository eventRepository, IEventServices eventServices, 
            ILogger<EventsController> logger)
        {
            _dayRepository = dayRepository;
            _eventRepository = eventRepository;
            _eventServices = eventServices;
            _logger = logger;

            _logger.LogInformation("EventsController created.");
        }


        public ActionResult Index()
        {
            try
            {
                var days = (List<Day>?)_dayRepository.GetAll();
                _ = _eventRepository.GetAll();

                // Filter the days that have upcoming events and order them by start time.
                days?.SortDays(DaySortKey.Date, eventSortKey: EventSortKey.StartTime);

                return View(days);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Index action.");
                throw;
            }
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Event newEvent)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _eventServices.CreateEvent(newEvent);
                    _logger.LogInformation("Event created successfully.");

                    return RedirectToAction("Index");
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "An error occurred in the Create action.");
                ModelState.AddModelError("Name", ex.Message);
            }

            return View(newEvent);
        }

        public ActionResult Edit(int id, string dayDate)
        {
            try
            {
                var referringUrl = Request.Headers["Referer"].ToString();

                ViewBag.ReferringUrl = referringUrl;

                DateOnly dayDateOnly = DateOnly.Parse(dayDate);
                var existingDay = _dayRepository.GetByDate(dayDateOnly);

                if (existingDay != null)
                {
                    var existingEvent = _eventRepository.GetById(id);
                    _logger.LogInformation("Edit action completed successfully.");

                    return View(existingEvent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Edit action.");
                throw new ArgumentException("Day not found/Events is null");
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult Edit(Event updatedEvent, DateOnly dayDate, string returnUrl)
        {
            var existingDay = _dayRepository.GetByDate(dayDate);

            try
            {
                if (ModelState.IsValid)
                {
                    _eventServices.EditEvent(existingDay, updatedEvent);

                    return Redirect(string.IsNullOrEmpty(returnUrl) ? Url.Action("Index", "Events") : returnUrl);
                }
                return View(updatedEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Edit action.");
            }
            return NotFound(ModelState);
        }

        public ActionResult Delete(int id, string dayDate)
        {
            try
            {
                var referringUrl = Request.Headers["Referer"].ToString();

                ViewBag.ReferringUrl = referringUrl;

                DateOnly dayDateOnly = DateOnly.Parse(dayDate);
                var existingDay = _dayRepository.GetByDate(dayDateOnly);

                if (existingDay != null)
                {
                    var existingEvent = _eventRepository.GetById(id);

                    return View(existingEvent);
                }
                else
                {
                    _logger.LogWarning("Day not found/Events is null in Delete action.");
                    throw new ArgumentException("Day not found/Events is null");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Delete action.");
            }
            return NotFound(id);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int dayId, string returnUrl)
        {
            try
            {
                var existingDay = _dayRepository.GetById(dayId);

                if (existingDay != null)
                {
                    var existingEvent = _eventRepository.GetById(id);

                    if (existingEvent != null)
                    {
                        _eventServices.DeleteEvent(existingEvent, existingDay);

                        return Redirect(string.IsNullOrEmpty(returnUrl) ? Url.Action("Index", "Events") : returnUrl);
                        //return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Delete action.");
            }
            return NotFound();
        }
    }
}

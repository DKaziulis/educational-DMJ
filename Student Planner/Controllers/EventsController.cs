using System.Net;
using Microsoft.AspNetCore.Mvc;
using Student_Planner.Models;
using Student_Planner.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Globalization;
using System.Composition;
using Student_Planner.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Student_Planner.Databases;
using Student_Planner.Repositories;
using Student_Planner.Repositories.Interfaces;

namespace Student_Planner.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventsDBContext _dbContext;
        private readonly IDayRepository _dayRepository;
        private readonly IEventRepository _eventRepository;
        private readonly EventServices _eventServices;
        public EventsController(EventsDBContext context, IDayRepository dayRepository, IEventRepository eventRepository, EventServices eventServices)
        {
            _dbContext = context;
            _dayRepository = dayRepository;
            _eventRepository = eventRepository;
            _eventServices = eventServices;
        }
        private static List<Event> events = new List<Event>();
        private static List<Day>? days = new List<Day>();

        public ActionResult Index()
        {
            days = (List<Day>?)_dayRepository.GetAll();
            events = _dbContext.Events.ToList();

            // Filter the days that have upcoming events and order them by start time.
            var dates = days.SortDays(DaySortKey.NumOfEvents, eventSortKey: EventSortKey.StartTime);
            
            return View(dates);
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

                    return RedirectToAction("Index");
                }
            }
            catch (ArgumentException ex)
            {
                // Catch the exception and set a custom error message in the ModelState
                ModelState.AddModelError("Name", ex.Message);
            }
            return View(newEvent);
        }

        public ActionResult Edit(int id, DateOnly dayDate)
        {
            //Checks for the correct Date, and event ID to edit the event
            var existingDay = _dayRepository.GetByDate(dayDate);
            if (existingDay != null)
            {
                var existingEvent = _eventRepository.GetById(id);

                return View(existingEvent);
            } 
            else 
            {
                throw new ArgumentException("Day not found/Events is null");
            }
        }

        [HttpPost]
        public ActionResult Edit(Event updatedEvent, DateOnly dayDate)
        {
            var existingDay = _dayRepository.GetByDate(dayDate); 
            if (ModelState.IsValid)
            {
                _eventServices.EditEvent(existingDay, updatedEvent);

                return RedirectToAction("Index");
            }
            return View(updatedEvent);
        }

        public ActionResult Delete(int id, DateOnly dayDate)
        {
            //Checks for the correct Date, and event ID to delete the event
            var existingDay = days.FirstOrDefault(d => d.Date == dayDate);
            if(existingDay != null && existingDay.events != null)
            {
                var existingEvent = existingDay.events.FirstOrDefault(e => e.Id == id);

                return View(existingEvent);
            }
            return NotFound();
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int dayId)
        {
            var existingDay = days.FirstOrDefault(d => d.Id == dayId);

            if (existingDay != null && existingDay.events != null)
            {
                var existingEvent = existingDay.events.FirstOrDefault(e => e.Id == id);
                if (existingEvent != null)
                {
                    _dbContext.Remove(existingEvent);
                    existingDay.events.Remove(existingEvent);
                    //updates the day's event list or deletes it if the list is empty
                    if (existingDay.events.Count == 0)
                    {
                        _dbContext.Remove(existingDay);
                    }
                    _dbContext.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}

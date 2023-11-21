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
        public EventsController(EventsDBContext context, IDayRepository dayRepository, IEventRepository eventRepository)
        {
            _dbContext = context;
            _dayRepository = dayRepository;
            _eventRepository = eventRepository;
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
                    //Takes the short date (yyyy-MM-dd) of the passed event, and converts it to DateOnly
                    newEvent.StartTime = TimeOnly.FromDateTime(newEvent.BeginDate);
                    DateOnly tempShortDate = DateOnly.FromDateTime(newEvent.BeginDate);

                //Checks which day to create the event for
                Day? updatedDay = DayOperator.FindDayForEvent(tempShortDate);

                //If such a date exists
                if (updatedDay != null && updatedDay.events != null)
                {
                    updatedDay.events.Add(newEvent);
                }
                
                //If there is no such date, creates a new one
                else
                {
                    updatedDay = new Day {
                        Id = Convert.ToInt32(tempShortDate.ToString("yyyyMMdd")),
                        Date = tempShortDate,
                        events = new List<Event> { newEvent }
                    };
                    _dbContext.Days.Add(updatedDay);
                }

                newEvent.DayId = updatedDay.Id;

                //Creates a unique ID, which consists of the event Date info, and the serial number of the day's events
                if (updatedDay.events != null)
                {
                    string customId = updatedDay.Id.ToString() + updatedDay.events.Count.ToString();
                    newEvent.Id = Convert.ToInt32(customId);

                    _dbContext.Events.Add(newEvent);
                    _dbContext.SaveChanges();
                }
                else
                {
                    return NotFound();
                }

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
            var existingDay = days.FirstOrDefault(d => d.Date == dayDate);
            if (existingDay != null && existingDay.events != null)
            {
                var existingEvent = existingDay.events.FirstOrDefault(e => e.Id == id);
                
                if (existingEvent == null)
                {
                    return NotFound();
                }

                return View(existingEvent);
            } 
            else 
            {  
                return NotFound(); 
            }
        }

        [HttpPost]
        public ActionResult Edit(Event? updatedEvent, DateOnly dayDate)
        {
            var existingDay = _dbContext.Days
                   .Include(d => d.events)
                   .FirstOrDefault(d => d.Date == dayDate);
            if (ModelState.IsValid)
            {
                 //days.FirstOrDefault(d => d.Date == dayId);
                //updates the event properties and saves to json file
                updatedEvent = EventOperator.EditEvent(existingDay, updatedEvent);

                _dbContext.SaveChanges();

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

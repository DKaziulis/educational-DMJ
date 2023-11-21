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
using Microsoft.VisualBasic;

namespace Student_Planner.Controllers
{
    public class EventsController : Controller
    {
        private readonly IDayRepository _dayRepository;
        private readonly IEventRepository _eventRepository;
        private readonly EventServices _eventServices;
        public EventsController(IDayRepository dayRepository, IEventRepository eventRepository, EventServices eventServices)
        {
            _dayRepository = dayRepository;
            _eventRepository = eventRepository;
            _eventServices = eventServices;
        }

        public ActionResult Index()
        {
            var days = (List<Day>?)_dayRepository.GetAll();
            _ = _eventRepository.GetAll();

            // Filter the days that have upcoming events and order them by start time.
            days?.SortDays(DaySortKey.Date, eventSortKey: EventSortKey.StartTime);
            
            return View(days);
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
                ModelState.AddModelError("Name", ex.Message);
            }
            return View(newEvent);
        }

        public ActionResult Edit(int id, DateOnly dayDate)
        {
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
            var existingDay = _dayRepository.GetByDate(dayDate);

            if(existingDay != null)
            {
                var existingEvent = _eventRepository.GetById(id);

                return View(existingEvent);
            }
            else
            {
                throw new ArgumentException("Day not found/Events is null");
            }
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int dayId)
        {
            var existingDay = _dayRepository.GetById(dayId);

            if (existingDay != null)
            {
                var existingEvent = _eventRepository.GetById(id);

                if (existingEvent != null)
                {
                    _eventServices.DeleteEvent(existingEvent, existingDay);

                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}

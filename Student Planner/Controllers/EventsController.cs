using System.Net;
using Microsoft.AspNetCore.Mvc;
using Student_Planner.Models;
using Student_Planner.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Composition;
using Student_Planner.Enums;

namespace Student_Planner.Controllers
{


    public class EventsController : Controller
    {
        public enum CourseGroup
        {
            Group1,
            Group2,
            Group3,
            Group4,
            Group5,
            AllGroups
        }

        private static List<Event> events = new List<Event>();
        private static List<Day> days = new List<Day>();
        private static readonly string eventDataFilePath = Path.GetFullPath(Path.Combine(
            Directory.GetCurrentDirectory(), "EventData"));
        private static string completePath = eventDataFilePath;
        private JsonHandler<Event> jsonHandler = new JsonHandler<Event>(dataFilePath: eventDataFilePath, list: events);

        public ActionResult Index()
        {
            days = DayHandler.LoadDays(days, eventDataFilePath);

            foreach (Day day in days)
            {
                completePath = Path.Combine(eventDataFilePath, string.Concat(day.Date.ToString("yyyy-MM-dd"), ".json"));
                day.events = jsonHandler.DeserializeFromJSON(completePath);
            }
            completePath = eventDataFilePath;

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
                    Day? updatedDay = DayHandler.FindDayForEvent(days, tempShortDate);

                    //If such a date exists, assigns the complete path to the date's file
                    if (updatedDay != null && updatedDay.events != null)
                    {
                        updatedDay.events.Add(newEvent);
                        completePath = Path.Combine(eventDataFilePath, string.Concat(updatedDay.Date.ToString("yyyy-MM-dd"), ".json"));
                    }

                    //If there is no such date, creates a new one, as well as the file for it, and
                    //assigns the new event data to that file
                    else
                    {
                        updatedDay = new Day
                        {
                            Date = tempShortDate,
                            events = new List<Event> { newEvent }
                        };
                        days.Add(updatedDay);
                        completePath = Path.Combine(completePath, string.Concat(updatedDay.Date.ToString("yyyy-MM-dd"), ".json"));
                        System.IO.File.Create(completePath).Close();
                    }

                    //Creates a unique ID, which consists of the event Date info, and the serial number of the day's events
                    if (updatedDay.events != null)
                    {
                        string customId = updatedDay.Date.ToString("yyyyMMdd") + updatedDay.events.Count.ToString();
                        newEvent.Id = Convert.ToInt32(customId); // unique ID

                        jsonHandler.SerializeToJson(completePath, updatedDay.events);
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
        

        public ActionResult Edit(int id, DateOnly dayId)
        {
            //Checks for the correct Date, and event ID to edit the event
            var existingDay = days.FirstOrDefault(d => d.Date == dayId);
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
        public ActionResult Edit(Event updatedEvent, DateOnly dayId)
        {
            if (ModelState.IsValid)
            {
                var existingDay = days.FirstOrDefault(d => d.Date == dayId);

                //updates the event properties and saves to json file
                updatedEvent = EventOperator.EditEvent(existingDay, updatedEvent, eventDataFilePath);

                return RedirectToAction("Index");
            }
            return View(updatedEvent);
        }


        public ActionResult Delete(int id, DateOnly dayId)
        {
            //Checks for the correct Date, and event ID to delete the event
            var existingDay = days.FirstOrDefault(d => d.Date == dayId);
            if(existingDay != null && existingDay.events != null)
            {
                var existingEvent = existingDay.events.FirstOrDefault(e => e.Id == id);

                return View(existingEvent);
            }
            return NotFound(); 
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, DateOnly dayId)
        {
            var existingDay = days.FirstOrDefault(d => d.Date == dayId);
            if (existingDay != null && existingDay.events != null)
            {
                var existingEvent = existingDay.events.FirstOrDefault(e => e.Id == id);
                if (existingEvent != null)
                {
                    //updates the day's event list or deletes it if the list is empty
                    existingDay = EventOperator.DeleteEvent(existingDay, existingEvent, eventDataFilePath);
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}

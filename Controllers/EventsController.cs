using System.Net;
using Microsoft.AspNetCore.Mvc;
using Student_Planner.Models;
using Student_Planner.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Composition;

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
        private string jsonData = "";
        private static readonly string eventDataFilePath = Path.GetFullPath(Path.Combine(
            Directory.GetCurrentDirectory(), "EventData"));
        private static string completePath = eventDataFilePath;
        private JsonControl<Event> jsonControl = new JsonControl<Event>(eventDataFilePath, events);

        public ActionResult Index()
        {
            events = jsonControl.DeserializeFromJSON(jsonData, eventDataFilePath, events);
            days = LoadDays(eventDataFilePath, events);

            foreach (Day day in days)
            {
                completePath = Path.Combine(eventDataFilePath, string.Concat(day.Date.ToString("yyyy-MM-dd"), ".json"));
                day.events = jsonControl.DeserializeFromJSON(jsonData, completePath, events);
            }
            completePath = eventDataFilePath;
            
            // Filter the days that have upcoming events and order them by start time.
            var dates = days
            .OrderBy(d => d.Date)
            .ToList();

            return View(dates);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Event newEvent)
        {
            if (ModelState.IsValid)
            {
                //Takes the short date (yyyy-MM-dd) of the passed event, and converts it to DateOnly
                newEvent.StartTime = TimeOnly.FromDateTime(newEvent.BeginDate);
                DateOnly tempShortDate = DateOnly.FromDateTime(newEvent.BeginDate);
                Day? updatedDay = null;

                //Checks which day to create the event for
                foreach(Day singleDay in days)
                {
                    if (singleDay.events != null && singleDay.Date == tempShortDate)
                    {
                        singleDay.events.Add(newEvent);
                        updatedDay = singleDay;
                        break;
                    }
                }

                //If such a date exists, assigns the complete path to the date's file
                if(updatedDay != null)
                    completePath = Path.Combine(eventDataFilePath, string.Concat(updatedDay.Date.ToString("yyyy-MM-dd"), ".json"));
                
                //If there is no such dates, creates a new one, as well as the file for it, and
                //assigns the new event data to that file
                else
                {
                    updatedDay = new Day {
                        Date = tempShortDate,
                        events = new List<Event> { newEvent }
                    };
                    days.Add(updatedDay);
                    completePath = Path.Combine(completePath, string.Concat(updatedDay.Date.ToString("yyyy-MM-dd"), ".json"));
                    System.IO.File.Create(completePath).Close();
                }

                //Creates a unique ID, which consists of the event Date info, and the serial number of the day's events
                if(updatedDay.events != null)
                {
                    string customId = updatedDay.Date.ToString("yyyyMMdd") + updatedDay.events.Count.ToString();
                    newEvent.Id = Convert.ToInt32(customId); // unique ID

                    jsonControl.SerializeToJson(jsonData, completePath, updatedDay.events);
                }
                else
                {
                    return NotFound();
                }

                return RedirectToAction("Index");
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
                if (existingDay != null && existingDay.events != null)
                {
                    var existingEvent = existingDay.events.FirstOrDefault(e => e.Id == updatedEvent.Id);
                    DateOnly tempShortDate = DateOnly.FromDateTime(updatedEvent.BeginDate);

                    if (existingEvent != null)
                    {
                        // Preserves the original day by setting the event's date to the existing day's date
                        updatedEvent.BeginDate = updatedEvent.BeginDate.Date.Add(existingEvent.BeginDate.TimeOfDay);

                        // Updates event properties
                        existingEvent.Name = updatedEvent.Name;
                        existingEvent.StartTime = updatedEvent.StartTime;
                        existingEvent.CourseGroup = updatedEvent.CourseGroup;
                        existingEvent.Description = updatedEvent.Description;

                        // Serialize the updated events in the same day's JSON file
                        string dayJsonFilePath = Path.Combine(eventDataFilePath, string.Concat(existingEvent.BeginDate.Date.ToString("yyyy-MM-dd"), ".json"));
                        jsonControl.SerializeToJson(jsonData, dayJsonFilePath, existingDay.events);
                    }
                    return RedirectToAction("Index");
                }
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
                    existingDay.events.Remove(existingEvent);
                    string dayJsonFilePath = Path.Combine(eventDataFilePath,
                        string.Concat(existingEvent.BeginDate.Date.ToString("yyyy-MM-dd"), ".json"));

                    if (existingDay.events.Count == 0)
                    {
                        System.IO.File.Delete(dayJsonFilePath);
                    }
                    else
                    {
                        jsonControl.SerializeToJson(jsonData, dayJsonFilePath, existingDay.events);
                    }
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        //Loads the list of all existing dates from json files in the EventData folder
        public List<Day> LoadDays(string filePath, List<Event> events)
        {
            string?[] files = Directory.GetFiles(filePath).Select(Path.GetFileName).ToArray();
            List<Day> days = new List<Day>();

            foreach (string? file in files)
            {
                if(file != null)
                {
                    Day loadDay = new()
                    {
                        Date = DateOnly.Parse(file.Remove(10, 5)),
                        events = events
                    };
                    days.Add(loadDay);
                }
            }
            return days;
        }
    }
}

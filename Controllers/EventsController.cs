using System.Net;
using Microsoft.AspNetCore.Mvc;
using Student_Planner.Models;
using Student_Planner.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Student_Planner.Controllers
{
    public class EventsController : Controller
    {
        private static List<Event> events = new List<Event>();
        private static List<Day> days = new List<Day>();
        private string jsonData = "";
        private static readonly string eventDataFilePath = Path.GetFullPath(Path.Combine(
            Directory.GetCurrentDirectory(), "EventData"));
        private static string completePath = eventDataFilePath;
        private JsonControl<Event> jsonControl = new JsonControl<Event>(eventDataFilePath, events);

        public List<Day> LoadDays(string filePath, List<Event> events)
        {
            string?[] files = Directory.GetFiles(filePath).Select(Path.GetFileName).ToArray();
            List<Day> days = new List<Day>();

            foreach (string file in files)
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

        public ActionResult Index()
        {
            events = jsonControl.DeserializeFromJSON(jsonData, eventDataFilePath, events);
            days = LoadDays(eventDataFilePath, events);

            foreach (Day day in days)
            {
                completePath = Path.Combine(eventDataFilePath, string.Concat(day.Date.ToString("yyyy-MM-dd"), ".json"));
                day.events = jsonControl.DeserializeFromJSON(jsonData, completePath, events);
                foreach (Event testEvent in day.events)
                {
                    Console.WriteLine(testEvent.Id);
                }
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
                newEvent.Id = events.Count + 1; // unique ID
                Console.WriteLine(newEvent.Id);
                DateOnly tempShortDate = DateOnly.FromDateTime(newEvent.StartTime);
                Day? updatedDay = null;
                foreach(Day singleDay in days)
                {
                    if (singleDay.Date == tempShortDate)
                    {
                        singleDay.events.Add(newEvent);
                        updatedDay = singleDay;
                        break;
                    }
                }
                if(updatedDay != null)
                    completePath = Path.Combine(eventDataFilePath, string.Concat(updatedDay.Date.ToString("yyyy-MM-dd"), ".json"));
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
                updatedDay.NumOfEvents++;

                jsonControl.SerializeToJson(jsonData, completePath, updatedDay.events);

                return RedirectToAction("Index");
            }
            return View(newEvent);
        }

        public ActionResult Edit(int id)
        {
            var existingEvent = events.FirstOrDefault(e => e.Id == id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            return View(existingEvent);
        }

        [HttpPost]
        public ActionResult Edit(Event updatedEvent)
        {
            if (ModelState.IsValid)
            {
                var existingEvent = events.FirstOrDefault(e => e.Id == updatedEvent.Id);
                DateOnly tempShortDate = DateOnly.FromDateTime(updatedEvent.StartTime);
                if (existingEvent != null)
                {
                    // Preserve the original day by setting the event's date to the existing day's date
                    updatedEvent.StartTime = updatedEvent.StartTime.Date.Add(existingEvent.StartTime.TimeOfDay);

                    // Update event properties.
                    existingEvent.Name = updatedEvent.Name;
                    existingEvent.StartTime = updatedEvent.StartTime;
                    existingEvent.Description = updatedEvent.Description;

                    // Serialize the updated events in the same day's JSON file
                    string dayJsonFilePath = Path.Combine(eventDataFilePath, string.Concat(existingEvent.StartTime.Date.ToString("yyyy-MM-dd"), ".json"));
                    jsonControl.SerializeToJson(jsonData, dayJsonFilePath, days.First(day => day.Date == tempShortDate).events);
                }
                return RedirectToAction("Index");
            }
            return View(updatedEvent);
        }


        public ActionResult Delete(int id)
        {
            var existingEvent = events.FirstOrDefault(e => e.Id == id);
            if (existingEvent == null)
            {
                return NotFound();
            }
            return View(existingEvent);
        }

 

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var existingEvent = events.FirstOrDefault(e => e.Id == id);
            if (existingEvent != null)
            {
                events.Remove(existingEvent);
                jsonControl.SerializeToJson(jsonData, eventDataFilePath, days[^1].events);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}

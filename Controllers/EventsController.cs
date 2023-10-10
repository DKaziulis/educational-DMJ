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
            string[] files = Directory.GetFiles(filePath).Select(Path.GetFileName).ToArray();
            List<Day> days = new List<Day>();

            foreach (string file in files)
            {
                if(file != null)
                {
                    Day loadDay = new Day();
                    loadDay.Date = DateOnly.Parse(file.Remove(10, 5));
                    loadDay.events = events;
                    days.Add(loadDay);
                }
            }
            return days;
        }

        public ActionResult Index()
        {
            //LINQ STUFF
            events = jsonControl.DeserializeFromJSON(jsonData, eventDataFilePath, events);
            days = LoadDays(eventDataFilePath, events);
            completePath = Path.Combine(completePath, string.Concat(days[^1].Date.ToString("yyyy-MM-dd"), ".json"));

            // Filter the days that have upcoming events and order them by start time.
            var dates = days
            .OrderBy(d => d.Date)
            .ToList();

            /*var upcomingEvents = events
                .Where(e => e.startTime >= today)
                .OrderBy(e => e.Name)
                .ToList();*/

            return View(dates);
        }

        public ActionResult AddDay()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDay(Day newDay)
        {
            if (ModelState.IsValid)
            {
                days.Add(newDay);
                completePath = Path.Combine(completePath, string.Concat(newDay.Date.ToString("yyyy-MM-dd"), ".json"));
                System.IO.File.Create(completePath).Close();

                return RedirectToAction("Index");
            }
            return View(newDay);
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
                events.Add(newEvent);

                completePath = Path.Combine(eventDataFilePath, string.Concat(days[^1].Date.ToString("yyyy-MM-dd"), ".json"));

                jsonControl.SerializeToJson(jsonData, completePath);

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
                if (existingEvent != null)
                {
                    // Update event properties.
                    existingEvent.Name = updatedEvent.Name;
                    existingEvent.StartTime = updatedEvent.StartTime;
                    existingEvent.Description = updatedEvent.Description;

                    jsonControl.SerializeToJson(jsonData, eventDataFilePath);
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
                jsonControl.SerializeToJson(jsonData, eventDataFilePath);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}

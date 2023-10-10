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
        private static readonly string eventDataFilePath = Path.GetFullPath(Path.Combine
            (Path.Combine(Directory.GetCurrentDirectory(), "EventData"), "Test.json"));
        private JsonControl<Event> jsonControl = new JsonControl<Event>(eventDataFilePath, events);

        public ActionResult Index()
        {
            //LINQ STUFF
            events = jsonControl.DeserializeFromJSON(jsonData, events);

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

                jsonControl.SerializeToJson(jsonData, eventDataFilePath);

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

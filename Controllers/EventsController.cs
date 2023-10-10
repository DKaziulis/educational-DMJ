using System.Net;
using Microsoft.AspNetCore.Mvc;
using Student_Planner.Models;
using Student_Planner.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Globalization;

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
            days.Add(new Day(Date: new DateOnly(2023, 10, 10), events));
            days.Add(new Day(Date: DateOnly.Parse("2023.10.11", CultureInfo.InvariantCulture), events));
            // Filter events that are upcoming and order them by start time.
            var today = DateTime.Today;
            var upcomingEvents = days
                //.Where(e => e.Date >= today)
                .OrderBy(e => e.Date)
                .ToList();

            return View(upcomingEvents);
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

                jsonControl.SerializeToJson(jsonData);

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

                    jsonControl.SerializeToJson(jsonData);
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
                jsonControl.SerializeToJson(jsonData);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}

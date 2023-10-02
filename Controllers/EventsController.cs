using System.Net;
using Microsoft.AspNetCore.Mvc;
using Student_Planner.Models;



namespace Student_Planner.Controllers
{
    public class EventsController : Controller
    {
        private static List<Event> events = new List<Event>();


        // GET: Events
        public ActionResult Index()
        {
            // Sort events by date before displaying.
            var sortedEvents = events.OrderBy(e => e.Date).ToList();
            return View(sortedEvents);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View("~/Views/Events/Create.cshtml");
        }

        // POST: Events/Create
        [HttpPost]
        public ActionResult Create(Event newEvent)
        {
            if (ModelState.IsValid)
            {
                newEvent.Id = events.Count + 1; // Assign a unique ID
                events.Add(newEvent);
                return RedirectToAction("Index");
            }
            return View(newEvent);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int id)
        {
            var existingEvent = events.FirstOrDefault(e => e.Id == id);
            if (existingEvent == null)
            {
                return NotFound();
            }
            return View(existingEvent);
        }

        // POST: Events/Edit/5
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
                    existingEvent.Date = updatedEvent.Date;
                    existingEvent.Description = updatedEvent.Description;
                }
                return RedirectToAction("Index");
            }
            return View(updatedEvent);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int id)
        {
            var existingEvent = events.FirstOrDefault(e => e.Id == id);
            if (existingEvent == null)
            {
                return NotFound();
            }
            return View(existingEvent);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var existingEvent = events.FirstOrDefault(e => e.Id == id);
            if (existingEvent != null)
            {
                events.Remove(existingEvent);
            }
            return RedirectToAction("Index");
        }


    }

}

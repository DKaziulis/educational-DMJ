﻿using System.Net;
using Microsoft.AspNetCore.Mvc;
using Student_Planner.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;

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
        private string jsonData = "";
        private static string eventDataFilePath = Path.GetFullPath(Path.Combine
            (Path.Combine(Directory.GetCurrentDirectory(), "EventData"), "Test.json"));

        //Method for serializing data into a JSON file in the EventData folder
        private void SerializeToJson(string jsonData)
        {
            // Serialize the list of events to JSON
            jsonData = JsonSerializer.Serialize(events);
            // Write the JSON data to the file
            System.IO.File.WriteAllText(eventDataFilePath, jsonData);
        }

        //Deserialize the JSON file data back into an events list if the file exists
        private List<Event> DeserializeFromJSON(string json, List<Event> events)
        {
            if (System.IO.File.Exists(eventDataFilePath))
            {
                jsonData = System.IO.File.ReadAllText(eventDataFilePath);

                if (!string.IsNullOrEmpty(jsonData))
                {
                    events = JsonSerializer.Deserialize<List<Event>>(jsonData);
                }
                else
                {
                    // Handle the case where jsonData is empty
                    events.Clear();
                }
            }
            return events;
        }

        public ActionResult Index()
        {
            //LINQ STUFF
            events = DeserializeFromJSON(jsonData, events);

            // Filter events that are upcoming and order them by start time.
            var today = DateTime.Today;
            var upcomingEvents = events
                .Where(e => e.StartTime >= today)
                .OrderBy(e => e.StartTime)
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

                SerializeToJson(jsonData);

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

                    SerializeToJson(jsonData);
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
                SerializeToJson(jsonData);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}

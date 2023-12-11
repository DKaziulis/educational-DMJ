using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_Planner.Databases;
using Student_Planner.DTO_s;
using Student_Planner.Models;
using Student_Planner.Repositories.Implementations;
using Student_Planner.Repositories.Interfaces;
using Student_Planner.Services.Implementations;
using Student_Planner.Services.Interfaces;

namespace Student_Planner.Controllers
{
    [ApiController]
    [Route("api/eventsapi")]
    public class EventsApiController : ControllerBase
    {
        private readonly IDayRepository _dayRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventServices _eventServices;

        public EventsApiController(IDayRepository dayRepository, IEventRepository eventRepository, IEventServices eventServices)
        {
            _dayRepository = dayRepository;
            _eventRepository = eventRepository;
            _eventServices = eventServices;
        }

        [HttpGet("allevents")]
        public ActionResult<IEnumerable<Event>> GetAllEvents()
        {
            try
            {
                var allEvents = _eventRepository.GetAll();
                return Ok(allEvents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpGet("eventbyid/{id}")]
        public ActionResult<Event> GetEventById(int id)
        {
            try
            {
                var myEvent = _eventRepository.GetById(id);
                return Ok(myEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpPost("addevent")]
        public ActionResult AddEvent([FromBody] EventDTO eventDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newEvent = new Event
                    {
                        Name = eventDTO.Name,
                        BeginDate = eventDTO.BeginDate,
                        StartTime = eventDTO.StartTime,
                        Description = eventDTO.Description
                    };

                    _eventServices.CreateEvent(newEvent);

                    return CreatedAtAction(nameof(GetEventById), new { id = newEvent.Id }, newEvent);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpPut("editevent/{id}")]
        public ActionResult EditEvent(int id, [FromBody] EventDTO eventDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Retrieve the existing event from the repository
                    var existingEvent = _eventRepository.GetById(id);
                    var existingDay = _dayRepository.GetById(existingEvent.DayId);

                    if (existingEvent == null)
                    {
                        return NotFound($"Event with ID {id} not found.");
                    }

                    // Update the properties of the existing event
                    existingEvent.Name = eventDTO.Name;
                    existingEvent.BeginDate = eventDTO.BeginDate;
                    existingEvent.StartTime = eventDTO.StartTime;
                    existingEvent.Description = eventDTO.Description;

                    // Perform the update in the service or repository
                    _eventServices.EditEvent(existingDay, existingEvent);

                    return Ok(existingEvent);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpDelete("deleteevent/{id}")]
        public ActionResult DeleteEvent(int id)
        {
            try
            {
                var existingEvent = _eventRepository.GetById(id);

                if (existingEvent != null)
                {
                    var existingDay = _dayRepository.GetById(existingEvent.DayId);

                    if (existingDay != null)
                    {
                        _eventServices.DeleteEvent(existingEvent, existingDay);
                    }
                    return NoContent();
                } 
                else
                {
                    return NotFound($"Event with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}

using Student_Planner.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Student_Planner.Services
{
    public static class Extension
    {
        public static List<Event> SortEvents(this List<Event> events)
        {
                return events.OrderBy(e => e.BeginDate).ToList();
        }
    }
}

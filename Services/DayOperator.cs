using Student_Planner.Models;

namespace Student_Planner.Services
{
    public class DayOperator
    {
        //Loads the list of all existing dates from json files in the specified File Path
        public static List<Day> LoadDays(List<Day> days, string filePath)
        {
            string?[] files = Directory.GetFiles(filePath).Select(Path.GetFileName).ToArray();
            days = new List<Day>();
            JsonHandler<Event> jsonHandler = new JsonHandler<Event>();

            foreach (string? file in files)
            {
                if (file != null)
                {
                    Day loadDay = new()
                    {
                        Date = DateOnly.Parse(file.Remove(10, 5)),
                        events = jsonHandler.DeserializeFromJSON(filePath)
                    };
                    days.Add(loadDay);
                }
            }
            return days;
        }

        public static Day? FindDayForEvent(List<Day> days, DateOnly shortDate)
        {
            Day? updatedDay = null;
            foreach (Day singleDay in days)
            {
                if (singleDay.events != null && singleDay.Date == shortDate)
                {
                    updatedDay = singleDay;
                    break;
                }
            }
            return updatedDay;
        }
    }
}

using Student_Planner.Models;

namespace Student_Planner.Services.Interfaces
{
    public interface IDayOperator
    {
        public Day? FindDayForEvent(DateOnly shortDate);
    }
}

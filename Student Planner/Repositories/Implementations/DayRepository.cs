using Microsoft.VisualBasic;
using Student_Planner.Databases;
using Student_Planner.Models;
using Student_Planner.Repositories.Interfaces;

namespace Student_Planner.Repositories.Implementations
{
    public class DayRepository : IDayRepository
    {
        private readonly EventsDBContext _dbContext;
        public DayRepository(EventsDBContext context) {
            _dbContext = context;
        }

        public IEnumerable<Day> GetAll() {
            return _dbContext.Days.ToList();
        }
        public Day? GetById(int id) {
            return _dbContext.Days.FirstOrDefault(d => d.Id == id);
        }
        public void Add(Day day)
        {
            _dbContext.Days.Add(day);
        }
        public void Update(Day day)
        {
            _dbContext.Days.Update(day);
        }
        public void Delete(Day day)
        {
            _dbContext.Days.Remove(day);
        }
        public void SaveChanges(){
            _dbContext.SaveChanges();
        }
        public Day GetByDate(DateOnly date)
        {
            return _dbContext.Days.FirstOrDefault(d => d.Date == date);
        }
    }
}

using Student_Planner.Databases;
using Student_Planner.Models;
using Student_Planner.Repositories.Interfaces;

namespace Student_Planner.Repositories.Implementations
{
    public class EventRepository : IEventRepository
    {
        private readonly EventsDBContext _dbContext;
        public EventRepository(EventsDBContext context)
        {
            _dbContext = context;
        }

        public IEnumerable<Event> GetAll()
        {
            return _dbContext.Events.ToList();
        }
        public Event? GetById(int id)
        {
            return _dbContext.Events.FirstOrDefault(e => e.Id == id);
        }
        public void Add(Event _event)
        {
            _dbContext.Events.Add(_event);
        }
        public void Update(Event _event)
        {
            _dbContext.Events.Update(_event);
        }
        public void Delete(Event _event)
        {
            _dbContext.Events.Remove(_event);
        }
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}

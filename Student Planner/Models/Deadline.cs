using System.ComponentModel.DataAnnotations;


namespace Student_Planner.Models
{
    public class Deadline : IComparable<Deadline>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public string Details { get; set; }

        public Deadline() { }

        public Deadline(int id,string title, DateTime dueDate, string details)
        {
            Id = id;
            Title = title;
            DueDate = dueDate;
            Details = details;
        }

        public int CompareTo(Deadline other)
        {
            if (other == null) return 1;
            return DueDate.CompareTo(other.DueDate);
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Student_Planner.DTO_s
{
    public class EventDTO
    {
        [MaxLength(60)]
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "BeginDate is required")]
        public DateTime BeginDate { get; set; }

        [Required(ErrorMessage = "StartTime is required")]
        public TimeOnly StartTime { get; set; }
        public string? Description { get; set; }
    }
}

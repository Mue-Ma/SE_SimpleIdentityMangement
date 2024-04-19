using System.ComponentModel.DataAnnotations;

namespace EventService.Client.Models
{
    public class Event
    {
        public Event()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

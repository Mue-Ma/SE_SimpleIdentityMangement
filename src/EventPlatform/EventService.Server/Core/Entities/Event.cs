using System.ComponentModel.DataAnnotations;

namespace EventService.Server.Core.Entities
{
    /// <summary>Represents an event</summary>
    public class Event
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}

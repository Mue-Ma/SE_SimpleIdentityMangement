using System.ComponentModel.DataAnnotations;

namespace EventService.Server.Core.Entities
{
    public class EventSubscription
    {
        public Guid Id { get; set; }
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public string EMail { get; set; } = string.Empty;
        public int Companions { get; set; }

    }
}

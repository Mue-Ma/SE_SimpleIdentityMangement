using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EventService.Server.Core.Entities
{
    /// <summary>Represents an subscription, every event can have multiple subscripions</summary>
    public class EventSubscription
    {
        public Guid Id { get; set; }
        [Required]
        public Guid EventId { get; set; }
        /// <summary>Gets set by identity</summary>
        public string EMail { get; set; } = string.Empty;
        [DefaultValue(0)]
        public int Companions { get; set; }
    }
}

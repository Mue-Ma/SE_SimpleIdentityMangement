namespace EventService.Client.Models
{
    public class EventSubscription
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string EMail { get; set; } = string.Empty;
        public int Companions { get; set; }
    }
}

namespace EventService.Server.Core.Entities
{
    public class Event
    {
        public int Id { get; set; }

        public string EventName { get; set; }

        public string EventType { get; set; } //Konzert, Fußballspiel, etc.

        public string EventLocation { get; set; }

        public DateTime EventDate { get; set; }

        public int AvailableTickets { get; set; }

        public decimal TicketPrice { get; set; }
    }
}

using EventService.Client.Models;

namespace EventService.Client.Services.Contracts
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetEvents();
        Task<Event> GetEventById(Guid id);
        Task CreateEvent(Event ev);
        Task UpdateEvent(Event ev);
        Task DeleteEvent(Guid id);
        Task<IEnumerable<Event>> GetEventByDescription(string description);
        Task<IEnumerable<EventSubscription>> GetSubscriptionsByIdentity();
        Task<EventSubscription?> GetSubscriptionByEventIdAndIdentity(Guid id);
        Task<IEnumerable<EventSubscription>> GetSubscriptionsByEventId(Guid id);
        Task<EventSubscription?> RegisterForEvent(EventSubscription subscription);
        Task UpdateSubscription(EventSubscription subscription);
        Task DeleteSubscription(Guid id);

    }
}
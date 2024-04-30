using EventService.Client.Models;

namespace EventService.Client.Services.Contracts
{
    public interface IEventService
    {
        Task CreateEvent(Event ev);
        Task DeleteEvent(Guid id);
        Task DeleteSubscription(Guid id);
        Task<EventSubscription?> GetSubscriptionByEventIdAndIdentity(Guid id);
        Task<IEnumerable<EventSubscription>> GetEventSubscriptionsByEventId(Guid id); 
        Task<Event> GetEventById(Guid id);
        Task<Event> GetEvents();
        Task<EventSubscription> RegisterForEvent(EventSubscription subscription);
        Task UpdateEvent(Event ev);
        Task UpdateSubscription(EventSubscription subscription);
    }
}
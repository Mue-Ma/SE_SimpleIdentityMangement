using EventService.Server.Core.Entities;

namespace EventService.Server.Persistence.Contracts
{
    public interface IEventSubscriptionRepository
    {
        Task Add(EventSubscription obj);
        Task AddMany(ICollection<EventSubscription> obj);
        Task Delete(Guid id);
        Task<IEnumerable<EventSubscription>> GetAll();
        Task<IEnumerable<EventSubscription>> GetEntityByEventId(Guid id);
        Task<EventSubscription?> GetEntityById(Guid id);
        Task Update(EventSubscription obj);
        Task<IEnumerable<EventSubscription>> GetEntityByEMail(string eMail);
    }
}
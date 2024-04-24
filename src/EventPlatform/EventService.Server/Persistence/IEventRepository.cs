using EventService.Server.Core.Entities;

namespace EventService.Server.Persistence
{
    public interface IEventRepository
    {
        Task AddMany(ICollection<Event> obj);
        Task Add(Event obj);
        Task Delete(Guid id);
        Task<IEnumerable<Event>> GetAll();
        Task<Event?> GetEntityById(Guid id);
        Task Update(Event obj);
        Task<Event?> GetByName(string name);
    }
}
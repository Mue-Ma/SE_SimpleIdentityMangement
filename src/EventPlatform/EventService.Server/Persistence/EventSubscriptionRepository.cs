using EventService.Server.Core.Entities;
using EventService.Server.Persistence.Contracts;
using MongoDB.Driver;

namespace EventService.Server.Persistence
{
    public class EventSubscriptionRepository : IEventSubscriptionRepository
    {
        protected readonly IDbContext Context;
        protected readonly IMongoCollection<EventSubscription> EntityDbSet;
        public EventSubscriptionRepository(IDbContext context)
        {
            Context = context;
            EntityDbSet = Context.GetCollection<EventSubscription>(typeof(EventSubscription).Name);
        }

        public async Task AddMany(ICollection<EventSubscription> obj)
        {
            await Task.Run(() => Context.AddCommand(() => EntityDbSet.InsertManyAsync(obj)));
            await Context.SaveChanges();
        }

        public async Task Add(EventSubscription obj)
        {
            await Task.Run(() => Context.AddCommand(() => EntityDbSet.InsertOneAsync(obj)));
            await Context.SaveChanges();
        }

        public async Task<IEnumerable<EventSubscription>> GetAll()
        {
            return await EntityDbSet.Find(Builders<EventSubscription>.Filter.Empty).ToListAsync();
        }

        public async Task<EventSubscription?> GetEntityById(Guid id)
        {
            return await EntityDbSet.Find(Builders<EventSubscription>.Filter.Eq(x => x.Id, id)).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<EventSubscription>> GetEntityByEventId(Guid id)
        {
            return await EntityDbSet.Find(Builders<EventSubscription>.Filter.Eq(x => x.EventId, id)).ToListAsync();
        }

        public async Task<IEnumerable<EventSubscription>> GetEntityByEMail(string eMail)
        {
            return await EntityDbSet.Find(Builders<EventSubscription>.Filter.Eq(x => x.EMail, eMail)).ToListAsync();
        }

        public async Task Update(EventSubscription obj)
        {
            await Task.Run(() => Context.AddCommand(() => EntityDbSet.ReplaceOneAsync(Builders<EventSubscription>.Filter.Eq(x => x.Id, obj.Id), obj)));
            await Context.SaveChanges();
        }

        public async Task Delete(Guid id)
        {
            await Task.Run(() => Context.AddCommand(() => EntityDbSet.DeleteOneAsync(Builders<EventSubscription>.Filter.Eq(x => x.Id, id))));
            await Context.SaveChanges();
        }
    }
}

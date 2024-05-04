using EventService.Server.Core.Entities;
using EventService.Server.Persistence.Contracts;
using MongoDB.Driver;

namespace EventService.Server.Persistence
{
    public class EventRepository : IEventRepository
    {
        protected readonly IDbContext Context;
        protected readonly IMongoCollection<Event> EntityDbSet;
        public EventRepository(IDbContext context)
        {
            Context = context;
            EntityDbSet = Context.GetCollection<Event>(typeof(Event).Name);
        }

        public async Task AddMany(ICollection<Event> obj)
        {
            await Task.Run(() => Context.AddCommand(() => EntityDbSet.InsertManyAsync(obj)));
            await Context.SaveChanges();
        }

        public async Task Add(Event obj)
        {
            await Task.Run(() => Context.AddCommand(() => EntityDbSet.InsertOneAsync(obj)));
            await Context.SaveChanges();
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            return await EntityDbSet.Find(Builders<Event>.Filter.Empty).ToListAsync();
        }

        public async Task<Event?> GetEntityById(Guid id)
        {
            return await EntityDbSet.Find(Builders<Event>.Filter.Eq(x => x.Id, id)).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Event>> GetEntityByFilter(string filter)
        {
            return await EntityDbSet.Find(Builders<Event>.Filter.Text(filter)).ToListAsync();
        }

        public async Task<Event?> GetByName(string name)
        {
            return await EntityDbSet.Find(Builders<Event>.Filter.Eq(x => x.Name, name)).SingleOrDefaultAsync();
        }

        public async Task Update(Event obj)
        {
            await Task.Run(() => Context.AddCommand(() => EntityDbSet.ReplaceOneAsync(Builders<Event>.Filter.Eq(x => x.Id, obj.Id), obj)));
            await Context.SaveChanges();
        }

        public async Task Delete(Guid id)
        {
            await Task.Run(() => Context.AddCommand(() => EntityDbSet.DeleteOneAsync(Builders<Event>.Filter.Eq(x => x.Id, id))));
            await Context.SaveChanges();
        }
    }
}

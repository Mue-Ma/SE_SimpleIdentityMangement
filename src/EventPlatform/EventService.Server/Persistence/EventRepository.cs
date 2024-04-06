using EventService.Server.Core.Entities;
using MongoDB.Driver;

namespace EventService.Server.Persistence
{
    public class EventRepository : IEventRepository
    {
        protected readonly IDbContext Context;
        protected readonly IMongoCollection<WeatherForecast> EntityDbSet;
        public EventRepository(IDbContext context)
        {
            Context = context;
            EntityDbSet = Context.GetCollection<WeatherForecast>(typeof(WeatherForecast).Name);
        }

        public async Task AddMany(ICollection<WeatherForecast> obj)
        {
            await Task.Run(() => Context.AddCommand(() => EntityDbSet.InsertManyAsync(obj)));
        }

        public async Task Add(WeatherForecast obj)
        {
            await Task.Run(() => Context.AddCommand(() => EntityDbSet.InsertOneAsync(obj)));
        }

        public async Task<IEnumerable<WeatherForecast>> GetAll()
        {
            return await EntityDbSet.Find(Builders<WeatherForecast>.Filter.Empty).ToListAsync();
        }

        public async Task<WeatherForecast?> GetEntityById(Guid id)
        {
            return await EntityDbSet.Find(Builders<WeatherForecast>.Filter.Eq(x => x.Id, id)).SingleOrDefaultAsync();
        }
        public async Task Update(WeatherForecast obj)
        {
            await Task.Run(() => Context.AddCommand(() => EntityDbSet.ReplaceOneAsync(Builders<WeatherForecast>.Filter.Eq(x => x.Id, obj.Id), obj)));
        }

        public async Task Delete(Guid id)
        {
            await Task.Run(() => Context.AddCommand(() => EntityDbSet.DeleteOneAsync(Builders<WeatherForecast>.Filter.Eq(x => x.Id, id))));
        }

    }
}

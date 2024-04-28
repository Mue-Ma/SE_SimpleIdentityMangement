using EventService.Server.Core.Configurations;
using EventService.Server.Persistence.Contracts;
using MongoDB.Driver;

namespace EventService.Server.Persistence
{
    public class DbContext(DatabaseConfiguration mongoConfiguration) : IDbContext
    {
        public MongoClient? MongoClient { get; set; }
        private IMongoDatabase? _database;
        private readonly List<Func<Task>> _commands = [];
        private readonly DatabaseConfiguration _mongoConfiguration = mongoConfiguration;

        public virtual async Task SaveChanges()
        {
            ConfigureMongo();

            try
            {
                var commandTasks = _commands.Select(c => c());
                await Task.WhenAll(commandTasks);
                _commands.Clear();
            }
            catch (Exception ex)
            {
                throw new MongoClientException(ex.Message);
            }
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            ConfigureMongo();
            return _database!.GetCollection<T>(name);
        }

        private void ConfigureMongo()
        {
            if (MongoClient != null) return;
            MongoClient = new MongoClient(_mongoConfiguration.ConnectionString);
            _database = MongoClient.GetDatabase(_mongoConfiguration.DatabaseName);
        }

        public void AddCommand(Func<Task> func) => _commands.Add(func);

        public void Dispose() => GC.SuppressFinalize(this);
    }
}

using MongoDB.Driver;

namespace EventService.Server.Persistence.Contracts
{
    public interface IDbContext
    {
        MongoClient? MongoClient { get; set; }

        void AddCommand(Func<Task> func);
        void Dispose();
        IMongoCollection<T> GetCollection<T>(string name);
        Task SaveChanges();
    }
}
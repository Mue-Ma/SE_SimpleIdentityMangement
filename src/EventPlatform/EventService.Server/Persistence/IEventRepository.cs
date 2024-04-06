using EventService.Server.Core.Entities;

namespace EventService.Server.Persistence
{
    public interface IEventRepository
    {
        Task AddMany(ICollection<WeatherForecast> obj);
        Task Add(WeatherForecast obj);
        Task Delete(Guid id);
        Task<IEnumerable<WeatherForecast>> GetAll();
        Task<WeatherForecast?> GetEntityById(Guid id);
        Task Update(WeatherForecast obj);
    }
}
using EventService.Server.Core.Entities;
using EventService.Server.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeatherForecastController(ILogger<WeatherForecastController> logger, IEventRepository eventRepository) : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger = logger;
        private readonly IEventRepository _eventRepository = eventRepository;

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            return await _eventRepository.GetAll();
        }
    }
}

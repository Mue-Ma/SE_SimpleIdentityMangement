using EventService.Server.Core.Entities;
using EventService.Server.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class WeatherForecastController(ILogger<WeatherForecastController> logger, IEventRepository eventRepository) : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger = logger;
        private readonly IEventRepository _eventRepository = eventRepository;

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            return await _eventRepository.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<WeatherForecast> Get(Guid id)
        {
            return await _eventRepository.GetEntityById(id) ?? new WeatherForecast();
        }
        //TODO add CRUD
    }
}

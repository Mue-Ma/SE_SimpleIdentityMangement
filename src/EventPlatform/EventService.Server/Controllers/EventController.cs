using EventService.Server.Core.Entities;
using EventService.Server.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class EventController(IEventRepository eventRepository) : ControllerBase
    {
        private readonly IEventRepository _eventRepository = eventRepository;

        [HttpGet]
        public async Task<IEnumerable<Event>> Get()
        {
            return await _eventRepository.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<Event> Get(Guid id)
        {
            return await _eventRepository.GetEntityById(id) ?? new Event();
        }

        [HttpPost]
        public async Task Post([FromBody] Event ev)
        {
            await _eventRepository.Add(ev);
        }

        [HttpPut]
        public async Task Put([FromBody] Event ev)
        {
            await _eventRepository.Update(ev);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _eventRepository.Delete(id);
        }
    }
}

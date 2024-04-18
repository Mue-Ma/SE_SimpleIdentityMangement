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
        public async Task<ActionResult<IEnumerable<Event>>> Get()
        {
            return Ok(await _eventRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> Get(Guid id)
        {
            var res = await _eventRepository.GetEntityById(id);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Post([FromBody] Event ev)
        {
            await _eventRepository.Add(ev);
            return Ok(ev.Id);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Event ev)
        {
            await _eventRepository.Update(ev);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _eventRepository.Delete(id);
            return NoContent();
        }
    }
}

using EventService.Server.Core.Entities;
using EventService.Server.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventController(IEventRepository eventRepository) : ControllerBase
    {
        private readonly IEventRepository _eventRepository = eventRepository;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Event>>> Get()
        {
            return Ok(await _eventRepository.GetAll());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Event>> Get(Guid id)
        {
            var res = await _eventRepository.GetEntityById(id);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Guid>> Post([FromBody] Event ev)
        {
            if (await _eventRepository.GetByName(ev.Name) != null) BadRequest("Eventname existiert bereits!");
            try 
            {
                await _eventRepository.Add(ev);
            }
            catch (Exception) 
            {
                if((await _eventRepository.GetEntityById(ev.Id)) != null) await _eventRepository.Delete(ev.Id);
                return Problem();
            }

            return Ok(ev.Id);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Put([FromBody] Event ev)
        {
            await _eventRepository.Update(ev);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _eventRepository.Delete(id);
            return NoContent();
        }
    }
}

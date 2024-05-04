using EventService.Server.Core.Entities;
using EventService.Server.Persistence.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class EventController(IEventRepository eventRepository) : ControllerBase
    {
        private readonly IEventRepository _eventRepository = eventRepository;

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Event>>> Get()
        {
            return Ok(await _eventRepository.GetAll());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Event>> Get(Guid id)
        {
            var res = await _eventRepository.GetEntityById(id);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Guid>> Post([FromBody] Event ev)
        {
            if (await _eventRepository.GetByName(ev.Name) != null) return BadRequest("Eventname existiert bereits!");
            
            await _eventRepository.Add(ev);
            var locationUri = $"{Request.Host}/Event/{ev.Id}";

            return Created(locationUri, ev);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Put([FromBody] Event ev)
        {
            if ((await Get(ev.Id)) == null) return BadRequest("No Element was found with the given id");
            await _eventRepository.Update(ev);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Delete(Guid id)
        {
            if((await Get(id)) == null) return BadRequest("No Element was found with the given id");
            await _eventRepository.Delete(id);
            return NoContent();
        }

        /// <summary>
        /// Returns events according to the filter string which applies to "name -> description"
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("[action]/{filter?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Event>>> GetByFilter(string filter = "")
        {
            if (string.IsNullOrEmpty(filter)) return await Get();
            return Ok(await _eventRepository.GetEntityByFilter(filter));
        }
    }
}

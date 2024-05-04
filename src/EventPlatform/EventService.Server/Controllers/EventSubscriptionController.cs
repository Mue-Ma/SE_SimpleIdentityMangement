using EventService.Server.Core.Entities;
using EventService.Server.Persistence.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Server.Controllers
{
    /// <response code="403">Admin role is required</response>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]

    public class EventSubscriptionController(IEventSubscriptionRepository eventSubscriptionRepository) : ControllerBase
    {
        private readonly IEventSubscriptionRepository _eventSubscriptionRepository = eventSubscriptionRepository;

        [HttpGet("[action]/{eMail}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<EventSubscription>>> GetByEMail(string eMail)
        {
            var res = await _eventSubscriptionRepository.GetEntityByEMail(eMail);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpGet("[action]/{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<EventSubscription>>> GetByEventId(Guid id)
        {
            var res = await _eventSubscriptionRepository.GetEntityByEventId(id);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpGet("[action]/{id}&&{eMail}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<EventSubscription>> GetByEventIdAndEmail(Guid id, string eMail)
        {
            var res = (await _eventSubscriptionRepository.GetEntityByEventId(id)).FirstOrDefault(s => s.EMail.Equals(eMail));
            return res != null ? Ok(res) : NotFound();
        }

        /// <summary>
        /// Returns all eventsubscriptions according to the identity name in the JWT
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventSubscription>>> GetByIdentity()
        {
            var res = await _eventSubscriptionRepository.GetEntityByEMail(User?.Identity?.Name ?? "");
            return res != null ? Ok(res) : NotFound();
        }

        /// <summary>
        /// Returns the event subscription according to the identity name in the JWT and the given EventId
        /// </summary>
        /// <param name="id"></param>

        [HttpGet("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<EventSubscription>> GetByEventIdAndIdentity(Guid id)
        {
            var res = (await _eventSubscriptionRepository.GetEntityByEMail(User?.Identity?.Name ?? "")).FirstOrDefault(s => s.EventId.Equals(id));
            return res != null ? Ok(res) : NotFound();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventSubscription"></param>
        /// <returns>Returns the newly created item and the uri of the ressource</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Event
        ///     {
        ///        "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "companions": 5
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item and the uri of the ressource</response>
        /// <response code="400">If their is o valid identity name or the subscription already exists</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Post([FromBody] EventSubscription eventSubscription)
        {
            if(string.IsNullOrEmpty(User?.Identity?.Name ?? "")) return BadRequest("No valid identity name");
            if ((await GetByEventIdAndIdentity(eventSubscription.EventId)).Value != null) return BadRequest("Subscription already exists");

            eventSubscription.EMail = User?.Identity?.Name!;
            await _eventSubscriptionRepository.Add(eventSubscription);
            var locationUri = $"{Request.Host}/EventSubscription/GetByEventIdAndIdentity/{eventSubscription.EventId}";

            return Created(locationUri, eventSubscription);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Put([FromBody] EventSubscription eventSubscription)
        {
            var sub = await _eventSubscriptionRepository.GetEntityById(eventSubscription.Id);
            if (sub == null) return BadRequest("No Element was found with the given id");
            if (!(User.IsInRole("admin") || (User.Identity?.Name ?? "") == sub.EMail)) return Forbid();

            await _eventSubscriptionRepository.Update(eventSubscription);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var sub = await _eventSubscriptionRepository.GetEntityById(id);
            if (sub == null) return BadRequest("No element was found with the given id");
            if(!(User.IsInRole("admin") || (User.Identity?.Name ?? "") == sub.EMail)) return Forbid();

            await _eventSubscriptionRepository.Delete(id);
            return NoContent();
        }
    }
}

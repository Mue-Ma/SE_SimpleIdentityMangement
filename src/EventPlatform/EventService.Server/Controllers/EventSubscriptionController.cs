using EventService.Server.Core.Entities;
using EventService.Server.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventSubscriptionController(IEventSubscriptionRepository eventSubscriptionRepository) : ControllerBase
    {
        private readonly IEventSubscriptionRepository _eventSubscriptionRepository = eventSubscriptionRepository;

        [HttpGet("[action]/{eMail}")]
        public async Task<ActionResult<IEnumerable<EventSubscription>>> GetByEMail(string eMail)
        {
            var res = await _eventSubscriptionRepository.GetEntityByEMail(eMail);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpGet("[action]/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<EventSubscription>>> GetByEventId(Guid id)
        {
            var res = await _eventSubscriptionRepository.GetEntityBySubscriptionId(id);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpGet("[action]/{id}&&{eMail}")]
        public async Task<ActionResult<EventSubscription>> GetByEventIdAndEmail(Guid id, string eMail)
        {
            var res = (await _eventSubscriptionRepository.GetEntityBySubscriptionId(id)).FirstOrDefault(s => s.EMail.Equals(eMail));
            return res != null ? Ok(res) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Post([FromBody] EventSubscription eventSubscription)
        {
            await _eventSubscriptionRepository.Add(eventSubscription);
            return eventSubscription.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] EventSubscription eventSubscription)
        {
            await _eventSubscriptionRepository.Update(eventSubscription);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _eventSubscriptionRepository.Delete(id);
            return NoContent();
        }
    }
}

﻿using EventService.Server.Core.Entities;
using EventService.Server.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventSubscriptionController(IEventSubscriptionRepository eventSubscriptionRepository) : ControllerBase
    {
        private readonly IEventSubscriptionRepository _eventSubscriptionRepository = eventSubscriptionRepository;

        [HttpGet("[action]/{eMail}")]
        public async Task<IEnumerable<EventSubscription>> GetByEMail(string eMail)
        {
            return await _eventSubscriptionRepository.GetEntityByEMail(eMail);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IEnumerable<EventSubscription>> GetByEventId(Guid id)
        {
            return await _eventSubscriptionRepository.GetEntityBySubscriptionId(id);
        }

        [HttpPost]
        public async Task Post([FromBody] EventSubscription eventSubscription)
        {
            await _eventSubscriptionRepository.Add(eventSubscription);
        }

        [HttpPut]
        public async Task Put([FromBody] EventSubscription eventSubscription)
        {
            await _eventSubscriptionRepository.Update(eventSubscription);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _eventSubscriptionRepository.Delete(id);
        }
    }
}
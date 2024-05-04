using BlazorBootstrap;
using EventService.Client.Models;
using EventService.Client.Services.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace EventService.Client.Services
{
    public class EventService(IHttpClientFactory httpClient, NavigationManager navigationManager,
        IIdentityService identityService, IMessageService messageService) : IEventService
    {
        private readonly IIdentityService _identityService = identityService;
        private readonly IMessageService _messageService = messageService;

        private readonly HttpClient _authHttpClient = httpClient.CreateClient("Authorized");
        private readonly HttpClient _publicHttpClient = httpClient.CreateClient("Public");
        private readonly NavigationManager _navigationManager = navigationManager;


        public async Task<IEnumerable<Event>> GetEvents()
        {
            return await _publicHttpClient.GetFromJsonAsync<IEnumerable<Event>>($"api/Event") ?? [];
        }

        public async Task<Event> GetEventById(Guid id)
        {
            return await _authHttpClient.GetFromJsonAsync<Event>($"api/Event/{id}")
                ?? throw new Exception("Event not found");
        }

        public async Task<IEnumerable<Event>> GetEventByFilter(string? filter)
        {
            return await _publicHttpClient.GetFromJsonAsync<IEnumerable<Event>>($"api/Event/GetByFilter/{filter}") ?? [];
        }

        public async Task CreateEvent(Event ev)
        {
            await _authHttpClient.PostAsJsonAsync("api/Event", ev);
        }

        public async Task UpdateEvent(Event ev)
        {
            await _authHttpClient.PutAsJsonAsync($"api/Event", ev);
        }

        public async Task DeleteEvent(Guid id)
        {
            await _authHttpClient.DeleteAsync($"api/Event/{id}");
        }

        public async Task<EventSubscription?> GetSubscriptionByEventIdAndIdentity(Guid subscriptionID)
        {
            var res = await _authHttpClient.GetAsync($"api/EventSubscription/GetByEventIdAndIdentity/{subscriptionID}");
            if (res.IsSuccessStatusCode) return await res.Content.ReadFromJsonAsync<EventSubscription>();
            else return null;
        }

        public async Task<IEnumerable<EventSubscription>> GetSubscriptionsByEventId(Guid id)
        {
            return (await _authHttpClient.GetFromJsonAsync<IEnumerable<EventSubscription>?>($"api/EventSubscription/GetByEventId/{id}"))
                ?? [];
        }

        public async Task<IEnumerable<EventSubscription>> GetSubscriptionsByIdentity()
        {
            if (!await _identityService.IsUser()) return new List<EventSubscription>();
            return await _authHttpClient.GetFromJsonAsync<IEnumerable<EventSubscription>>($"api/EventSubscription/GetByIdentity") ?? [];
        }

        public async Task<EventSubscription?> RegisterForEvent(EventSubscription subscription)
        {
            var res = await _authHttpClient.PostAsJsonAsync("api/EventSubscription", subscription);

            subscription = await res.Content.ReadFromJsonAsync<EventSubscription>()
                ?? throw new Exception("Added subscription not found");

            return subscription;
        }

        public async Task DeleteSubscription(Guid subscriptionID)
        {
            await _authHttpClient.DeleteAsync($"api/EventSubscription/{subscriptionID}");
        }

        public async Task UpdateSubscription(EventSubscription subscription)
        {
            await _authHttpClient.PutAsJsonAsync("api/EventSubscription", subscription);
        }
    }
}

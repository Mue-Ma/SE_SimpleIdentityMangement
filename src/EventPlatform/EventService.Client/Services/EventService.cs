﻿using BlazorBootstrap;
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


        public async Task<Event> GetEvents()
        {
            return await _publicHttpClient.GetFromJsonAsync<Event>($"http://localhost/eventservice/api/Event") ?? throw new Exception("Event not found");
        }

        public async Task<Event> GetEventById(Guid id)
        {
            return await _authHttpClient.GetFromJsonAsync<Event>($"http://localhost/eventservice/api/Event/{id}") ?? throw new Exception("Event not found");
        }

        public async Task CreateEvent(Event ev)
        {
            var response = await _authHttpClient.PostAsJsonAsync("http://localhost/eventservice/api/Event", ev);
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = $"Error: {response.StatusCode}";
                _navigationManager.NavigateTo($"/ErrorPages/ErrorPage?message={Uri.EscapeDataString(errorMessage)}");
            }
        }
        public async Task UpdateEvent(Event ev)
        {
            var res = await _authHttpClient.PutAsJsonAsync($"http://localhost/eventservice/api/Event", ev);
            if (res.IsSuccessStatusCode) _messageService.ShowMessage(ToastType.Success, $"Änderung des Events war erfolgreich");
            else _messageService.ShowMessage(ToastType.Danger, $"Etwas is bei der Änderung schief gegangen.");
        }

        public async Task DeleteEvent(Guid id)
        {

            var deleteResponse = await _authHttpClient.DeleteAsync($"http://localhost/eventservice/api/Event/{id}");
            if (!deleteResponse.IsSuccessStatusCode)
            {
                string errorMessage = $"Error: {deleteResponse.StatusCode}";
                _navigationManager.NavigateTo($"/ErrorPages/ErrorPage?message={Uri.EscapeDataString(errorMessage)}");
            }
        }

        public async Task<EventSubscription> RegisterForEvent(EventSubscription subscription)
        {
            EventSubscription ret;
            subscription.EMail = await _identityService.GetIdentityName() ?? throw new Exception("Identity name is null");
            var res = await _authHttpClient.PostAsJsonAsync("http://localhost/eventservice/api/EventSubscription", subscription);

            if (res.IsSuccessStatusCode)
            {
                ret = await _authHttpClient.GetFromJsonAsync<EventSubscription>($"http://localhost/eventservice/api/EventSubscription/GetByEventIdAndIdentity/{subscription.EventId}")
                    ?? throw new Exception("Added subscription not found");
                _messageService.ShowMessage(ToastType.Success, $"Registrierung für {subscription?.Companions + 1} Personen war erfolgreich");
            }
            else
            {
                _messageService.ShowMessage(ToastType.Danger, $"Etwas is bei der Registrierung schief gegangen.");
                throw new Exception("Registration was unsuccessful");
            }

            return ret;
        }

        public async Task DeleteSubscription(Guid subscriptionID)
        {
            var res = await _authHttpClient.DeleteAsync($"http://localhost/eventservice/api/EventSubscription/{subscriptionID}");

            if (res.IsSuccessStatusCode) _messageService.ShowMessage(ToastType.Success, $"Abmeldung der Registrierung {subscriptionID} war erfolgreich");
            else _messageService.ShowMessage(ToastType.Danger, $"Etwas is bei der Abmeldung schief gegangen.");
        }

        public async Task UpdateSubscription(EventSubscription subscription)
        {
            var res = await _authHttpClient.PutAsJsonAsync("http://localhost/eventservice/api/EventSubscription", subscription);

            if (res.IsSuccessStatusCode) _messageService.ShowMessage(ToastType.Success, $"Änderung der Beigelieter auf {subscription?.Companions + 1} Personen war erfolgreich");
            else _messageService.ShowMessage(ToastType.Danger, $"Etwas is bei der Änderung schief gegangen.");
        }

        public async Task<EventSubscription?> GetSubscriptionByEventIdAndIdentity(Guid subscriptionID)
        {
            return await _authHttpClient.GetFromJsonAsync<EventSubscription>($"http://localhost/eventservice/api/EventSubscription/GetByEventIdAndIdentity/{subscriptionID}");
        }

        public async Task<IEnumerable<EventSubscription>> GetEventSubscriptionsByEventId(Guid id)
        {
            return (await _authHttpClient.GetFromJsonAsync<IEnumerable<EventSubscription>?>($"http://localhost/eventservice/api/EventSubscription/GetByEventId/{id}"))
                ?? new List<EventSubscription>();
        }
    }
}

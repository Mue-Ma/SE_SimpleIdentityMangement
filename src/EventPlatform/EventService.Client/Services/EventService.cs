using BlazorBootstrap;
using EventService.Client.Models;
using EventService.Client.Services.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace EventService.Client.Services
{
    public class EventService(HttpClient httpClient, NavigationManager navigationManager, IIdentityService identityService)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly NavigationManager _navigationManager = navigationManager;
        private readonly IIdentityService _identityService = identityService;

        public async Task<Event> GetEventById(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Event>($"http://localhost/eventservice/api/Event/{id}") ?? throw new Exception("Event not found");
        }

        public async Task CreateEvent(Event ev)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost/eventservice/api/Event", ev);
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = $"Error: {response.StatusCode}";
                _navigationManager.NavigateTo($"/ErrorPages/ErrorPage?message={Uri.EscapeDataString(errorMessage)}");
            }
        }
        private async Task UpdateEvent(Event ev)
        {
            var res = await _httpClient.PutAsJsonAsync($"http://localhost/eventservice/api/Event", ev);
            if (res.IsSuccessStatusCode) ShowMessage(ToastType.Success, $"Änderung des Events war erfolgreich");
            else ShowMessage(ToastType.Danger, $"Etwas is bei der Änderung schief gegangen.");
        }

        private async Task DeleteEvent(Guid id)
        {

            var deleteResponse = await _httpClient.DeleteAsync($"http://localhost/eventservice/api/Event/{id}");
            if (!deleteResponse.IsSuccessStatusCode)
            {
                string errorMessage = $"Error: {deleteResponse.StatusCode}";
                _navigationManager.NavigateTo($"/ErrorPages/ErrorPage?message={Uri.EscapeDataString(errorMessage)}");
            }
        }

        private async Task<EventSubscription> RegisterForEvent(EventSubscription subscription)
        {
            EventSubscription ret;
            subscription.EMail = await _identityService.GetIdentityName() ?? throw new Exception("Identity name is null");
            var res = await _httpClient.PostAsJsonAsync("http://localhost/eventservice/api/EventSubscription", subscription);

            if (res.IsSuccessStatusCode)
            {
                ret = await _httpClient.GetFromJsonAsync<EventSubscription>($"http://localhost/eventservice/api/EventSubscription/GetByEventIdAndIdentity/{subscription.EventId}")
                    ?? throw new Exception("Added subscription not found");
                ShowMessage(ToastType.Success, $"Registrierung für {subscription?.Companions + 1} Personen war erfolgreich");
            }
            else
            {
                ShowMessage(ToastType.Danger, $"Etwas is bei der Registrierung schief gegangen.");
                throw new Exception("Registration was unsuccessful");
            } 

            return ret;
        }

        private async Task DeleteSubscription(Guid subscriptionID)
        {
            var res = await _httpClient.DeleteAsync($"http://localhost/eventservice/api/EventSubscription/{subscriptionID}");

            if (res.IsSuccessStatusCode) ShowMessage(ToastType.Success, $"Abmeldung der Registrierung {subscriptionID} war erfolgreich");
            else ShowMessage(ToastType.Danger, $"Etwas is bei der Abmeldung schief gegangen.");
        }

        private async Task UpdateSubscription(EventSubscription subscription)
        {
            var res = await _httpClient.PutAsJsonAsync("http://localhost/eventservice/api/EventSubscription", subscription);

            if (res.IsSuccessStatusCode) ShowMessage(ToastType.Success, $"Änderung der Beigelieter auf {subscription?.Companions + 1} Personen war erfolgreich");
            else ShowMessage(ToastType.Danger, $"Etwas is bei der Änderung schief gegangen.");
        }

        private void ShowMessage(ToastType success, string v)
        {
            throw new NotImplementedException();
        }
    }
}

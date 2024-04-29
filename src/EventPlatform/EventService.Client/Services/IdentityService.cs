using EventService.Client.Services.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace EventService.Client.Services
{
    public class IdentityService(AuthenticationStateProvider authenticationStateProvider) : IIdentityService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

        public async Task<ClaimsPrincipal> GetClaimsPrincipal()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authState.User;
        }

        public async Task<string?> GetIdentityName()
        {
            return (await GetClaimsPrincipal()).Identity?.Name;
        }
    }
}

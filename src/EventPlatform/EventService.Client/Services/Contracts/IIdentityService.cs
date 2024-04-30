using System.Security.Claims;

namespace EventService.Client.Services.Contracts
{
    public interface IIdentityService
    {
        Task<ClaimsPrincipal> GetClaimsPrincipal();
        Task<string?> GetIdentityName();
        Task<bool> IsAdmin();
        Task<bool> IsUser();
        Task<bool> IsAuthenticated();
    }
}
using System.Security.Claims;

namespace EventService.Client.Services.Contracts
{
    public interface IIdentityService
    {
        Task<ClaimsPrincipal> GetClaimsPrincipal();
        Task<string?> GetIdentityName();
    }
}
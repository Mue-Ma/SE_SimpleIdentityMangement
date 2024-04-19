using EventPlatform.Common.Core.Utils;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Security.Claims;

namespace EventPlatform.Common.Core.Factories
{
    public class KeyCloakAccountFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        public KeyCloakAccountFactory(IAccessTokenProviderAccessor accessor) : base(accessor)
        {
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
               RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var initialUser = await base.CreateUserAsync(account, options);
            return initialUser.Identity != null && initialUser.Identity.IsAuthenticated
                ? await KeycloakClaimsHelper.TransformAsync(initialUser)
                : initialUser;
        }
    }
}

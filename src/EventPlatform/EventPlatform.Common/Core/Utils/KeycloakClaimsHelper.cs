using Newtonsoft.Json;
using System.Security.Claims;

namespace EventPlatform.Common.Core.Utils
{
    public static class KeycloakClaimsHelper
    {
        /// <summary>
        /// Bietet einen zentralen Transformationspunkt, um den angegebenen Principal zu ändern.
        /// Note: Dies wird bei jedem AuthenticateAsync-Aufruf ausgeführt, daher ist es sicherer, einen
        /// einen neuen ClaimsPrincipal zurückzugeben, wenn Ihre Transformation nicht identisch ist.
        /// </summary>
        public static Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var result = principal.Clone();
            if (result.Identity is not ClaimsIdentity identity)
            {
                return Task.FromResult(result);
            }

            var realmAccessClaim = principal.FindFirst((claim) => claim.Type == "roles");
            var clientRoles = JsonConvert.DeserializeObject<string[]>(realmAccessClaim!.Value);

            foreach (var role in clientRoles!)
            {
                var value = role;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    identity.AddClaim(new Claim("roles", value));
                }
            }

            return Task.FromResult(result);
        }
    }
}

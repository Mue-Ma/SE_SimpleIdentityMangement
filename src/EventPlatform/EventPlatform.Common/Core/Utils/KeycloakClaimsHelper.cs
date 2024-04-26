using Newtonsoft.Json.Linq;
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

            var resourceAccessValue = principal.FindFirst("realm_access")?.Value;
            if (String.IsNullOrWhiteSpace(resourceAccessValue))
            {
                return Task.FromResult(result);
            }

            JArray jsonArray = JArray.Parse(resourceAccessValue);

            // Extract roles
            foreach (JObject jsonObject in jsonArray.Cast<JObject>())
            {
                if (jsonObject.TryGetValue("roles", out JToken? rolesToken) && rolesToken is JArray rolesArray)
                {
                    foreach (var role in rolesArray)
                    {
                        identity.AddClaim(new Claim("roles", role.Value<string>()!));
                    }
                }
            }

            return Task.FromResult(result);
        }
    }
}

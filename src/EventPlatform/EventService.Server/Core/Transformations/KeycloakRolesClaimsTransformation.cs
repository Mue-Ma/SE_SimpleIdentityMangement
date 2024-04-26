using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace EventService.Server.Core.Transformations
{
    public class KeycloakRolesClaimsTransformation : IClaimsTransformation
    {
        /// <summary>
        /// Bietet einen zentralen Transformationspunkt, um den angegebenen Principal zu ändern.
        /// Note: Dies wird bei jedem AuthenticateAsync-Aufruf ausgeführt, daher ist es sicherer, einen
        /// einen neuen ClaimsPrincipal zurückzugeben, wenn Ihre Transformation nicht identisch ist.
        /// </summary>
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            return await EventPlatform.Common.Core.Utils.KeycloakClaimsHelper.TransformAsync(principal);
        }
    }
}

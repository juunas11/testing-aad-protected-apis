using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Joonasw.AadTestingDemo.API.Authorization
{
    internal class ScopeClaimSplitTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            Claim[] scopeClaims = principal.FindAll(Claims.ScopeClaimType).ToArray();
            if (scopeClaims.Length == 0 || scopeClaims.Length > 1)
            {
                // No claims for scopes exist or more than one exist
                // The transformation can be run more than once,
                // in which we would possible have more than one claim at this point
                // Anyway, we don't need to do anything now.
                return Task.FromResult(principal);
            }

            // At this point we know there is one value
            Claim claim = scopeClaims[0];

            if (!scopeClaims[0].Value.Contains(' '))
            {
                // The value is a single scope, nothing to split
                return Task.FromResult(principal);
            }

            string[] scopes = claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            IEnumerable<Claim> claims = scopes.Select(s => new Claim(Claims.ScopeClaimType, s));

            return Task.FromResult(new ClaimsPrincipal(new ClaimsIdentity(principal.Identity, claims)));
        }
    }
}

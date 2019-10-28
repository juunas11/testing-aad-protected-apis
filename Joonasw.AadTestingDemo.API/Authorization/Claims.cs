using System.Security.Claims;

namespace Joonasw.AadTestingDemo.API.Authorization
{
    internal static class Claims
    {
        internal const string ScopeClaimType = "http://schemas.microsoft.com/identity/claims/scope";
        internal const string AppPermissionOrRolesClaimType = ClaimTypes.Role;
    }
}

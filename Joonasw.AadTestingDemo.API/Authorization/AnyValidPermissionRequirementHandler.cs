using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Joonasw.AadTestingDemo.API.Authorization
{
    internal class AnyValidPermissionRequirementHandler : AuthorizationHandler<AnyValidPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AnyValidPermissionRequirement requirement)
        {
            // Checks caller has at least one valid permission
            string[] delegatedPermissions = context.User.FindAll(Claims.ScopeClaimType).Select(c => c.Value).ToArray();
            string[] allAcceptedPermissions = DelegatedPermissions.All;
            if (delegatedPermissions.Any(p => allAcceptedPermissions.Contains(p)))
            {
                // Caller has a valid delegated permission
                // If your API has different user roles,
                // this is where you would check that, before calling context.Succeed()
                context.Succeed(requirement);
            }

            // If we reached here without calling context.Succeed(),
            // the call will fail with a 403 Forbidden
            return Task.CompletedTask;
        }
    }
}

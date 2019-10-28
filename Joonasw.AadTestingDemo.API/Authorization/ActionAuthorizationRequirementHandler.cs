using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Joonasw.AadTestingDemo.API.Authorization
{
    public class ActionAuthorizationRequirementHandler : AuthorizationHandler<ActionAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ActionAuthorizationRequirement requirement)
        {
            var delegatedPermissions = context.User.FindAll(Claims.ScopeClaimType).Select(c => c.Value).ToArray();
            var acceptedDelegatedPermissions = AuthorizedPermissions.DelegatedPermissionsForActions[requirement.Action];
            if (acceptedDelegatedPermissions.Any(accepted => delegatedPermissions.Any(available => accepted == available)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Joonasw.AadTestingDemo.API.Authorization
{
    internal class ActionAuthorizationRequirementHandler : AuthorizationHandler<ActionAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ActionAuthorizationRequirement requirement)
        {
            // Checks the user has a permission accepted for this action
            string[] delegatedPermissions = context.User.FindAll(Claims.ScopeClaimType).Select(c => c.Value).ToArray();
            string[] acceptedDelegatedPermissions = AuthorizedPermissions.DelegatedPermissionsForActions[requirement.Action];
            string[] appPermissionsOrRoles = context.User.FindAll(Claims.AppPermissionOrRolesClaimType).Select(c => c.Value).ToArray();
            string[] acceptedApplicationPermissions = AuthorizedPermissions.ApplicationPermissionsForActions[requirement.Action];

            if (acceptedDelegatedPermissions.Any(accepted => delegatedPermissions.Contains(accepted)))
            {
                context.Succeed(requirement);
            }
            else if (acceptedApplicationPermissions.Any(accepted => appPermissionsOrRoles.Contains(accepted)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

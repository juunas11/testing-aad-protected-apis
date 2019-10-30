using Microsoft.AspNetCore.Authorization;

namespace Joonasw.AadTestingDemo.API.Authorization
{
    internal class ActionAuthorizationRequirement : IAuthorizationRequirement
    {
        public ActionAuthorizationRequirement(string action)
        {
            Action = action;
        }

        public string Action { get; }
    }
}

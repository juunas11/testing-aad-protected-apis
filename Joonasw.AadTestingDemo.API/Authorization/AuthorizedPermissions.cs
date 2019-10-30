using System.Collections.Generic;

namespace Joonasw.AadTestingDemo.API.Authorization
{
    internal static class AuthorizedPermissions
    {
        /// <summary>
        /// Contains the allowed delegated permissions for each action.
        /// If the caller has one of the allowed ones, they should be allowed
        /// to perform the action.
        /// </summary>
        public static IReadOnlyDictionary<string, string[]> DelegatedPermissionsForActions = new Dictionary<string, string[]>
        {
            [Actions.ReadThings] = new[] { DelegatedPermissions.ReadThings },
            [Actions.ReadOtherThings] = new[] { DelegatedPermissions.ReadOtherThings }
        };
    }
}

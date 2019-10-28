using System.Collections.Generic;

namespace Joonasw.AadTestingDemo.API.Authorization
{
    public static class AuthorizedPermissions
    {
        public static IReadOnlyDictionary<string, string[]> DelegatedPermissionsForActions = new Dictionary<string, string[]>
        {
            [Actions.ReadThings] = new[] { DelegatedPermissions.ReadThings }
        };
    }
}

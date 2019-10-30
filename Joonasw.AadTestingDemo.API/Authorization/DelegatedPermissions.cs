using System.Linq;

namespace Joonasw.AadTestingDemo.API.Authorization
{
    internal static class DelegatedPermissions
    {
        public const string ReadThings = "Things.Read";
        public const string ReadOtherThings = "OtherThings.Read";

        public static string[] All => typeof(DelegatedPermissions)
            .GetFields()
            .Where(f => f.Name != nameof(All))
            .Select(f => f.GetValue(null) as string)
            .ToArray();
    }
}

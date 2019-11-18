using System.Linq;

namespace Joonasw.AadTestingDemo.API.Authorization
{
    internal static class ApplicationPermissions
    {
        public const string ReadAllThings = "Things.Read.All";
        public const string ReadAllOtherThings = "OtherThings.Read.All";

        public static string[] All => typeof(ApplicationPermissions)
            .GetFields()
            .Where(f => f.Name != nameof(All))
            .Select(f => f.GetValue(null) as string)
            .ToArray();
    }
}

using System.Linq;

namespace Joonasw.AadTestingDemo.API.Authorization
{
    /// <summary>
    /// Actions that can be done on the API
    /// </summary>
    internal static class Actions
    {
        public const string ReadThings = "Things/Read";
        public const string ReadOtherThings = "OtherThings/Read";

        public static string[] All => typeof(Actions)
            .GetFields()
            .Where(f => f.Name != nameof(All))
            .Select(f => f.GetValue(null) as string)
            .ToArray();
    }
}

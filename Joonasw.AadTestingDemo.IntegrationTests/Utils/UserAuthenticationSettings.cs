namespace Joonasw.AadTestingDemo.IntegrationTests.Utils
{
    /// <summary>
    /// Settings for authenticating a test
    /// request as a user
    /// </summary>
    public class UserAuthenticationSettings
    {
        /// <summary>
        /// The URL to acquire the access token from,
        /// e.g. https://login.microsoftonline.com/your-aad-tenant-id/oauth2/v2.0/token
        /// </summary>
        public string TokenUrl { get; set; }
        /// <summary>
        /// Client id / application id for the
        /// registered test app
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Secret for the registered test app
        /// </summary>
        public string ClientSecret { get; set; }
        /// <summary>
        /// Username of a user registered in AAD.
        /// Should be from a test tenant, not a production one.
        /// Cannot have MFA enabled.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password of the user
        /// </summary>
        public string Password { get; set; }
    }
}
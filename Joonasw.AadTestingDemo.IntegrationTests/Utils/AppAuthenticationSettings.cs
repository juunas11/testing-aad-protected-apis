namespace Joonasw.AadTestingDemo.IntegrationTests.Utils
{
    /// <summary>
    /// Settings for authenticating a test
    /// request as an app
    /// </summary>
    public class AppAuthenticationSettings
    {
        /// <summary>
        /// Client id / application id for the
        /// registered test app
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Secret for the registered test app
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
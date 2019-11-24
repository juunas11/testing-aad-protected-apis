namespace Joonasw.AadTestingDemo.IntegrationTests.Utils
{
    public class IntegrationTestSettings
    {
        /// <summary>
        /// Base URL of the Azure Key Vault
        /// containing settings for tests,
        /// including authentication credentials.
        /// </summary>
        public string KeyVaultUrl { get; set; }
        /// <summary>
        /// The Azure AD authority, e.g. https://login.microsoftonline.com/your-aad-tenant-id/v2.0
        /// </summary>
        public string Authority { get; set; }
        /// <summary>
        /// The App ID URI for the API app registration,
        /// e.g. api://some-guid-generated-by-aad
        /// </summary>
        public string ApiAppIdUri { get; set; }
        public UserAuthenticationSettings UserAuthentication { get; set; }
        public AppAuthenticationSettings AppAuthentication { get; set; }
    }
}
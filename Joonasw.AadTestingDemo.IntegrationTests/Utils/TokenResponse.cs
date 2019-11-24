using Newtonsoft.Json;

namespace Joonasw.AadTestingDemo.IntegrationTests.Utils
{
    class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}

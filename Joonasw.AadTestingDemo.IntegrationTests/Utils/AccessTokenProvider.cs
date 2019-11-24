using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Joonasw.AadTestingDemo.IntegrationTests.Utils
{
    /// <summary>
    /// Authenticates requests as a user / as an app
    /// for integration tests.
    /// </summary>
    public class AccessTokenProvider
    {
        private readonly IntegrationTestSettings _settings;
        private readonly IConfidentialClientApplication _confidentialClientApp;
        private readonly HttpClient _client;

        public AccessTokenProvider(IntegrationTestSettings settings)
        {
            _settings = settings;
            _confidentialClientApp = ConfidentialClientApplicationBuilder
                .Create(settings.AppAuthentication.ClientId)
                .WithClientSecret(settings.AppAuthentication.ClientSecret)
                .WithAuthority(settings.Authority)
                .Build();
            _client = new HttpClient();
        }

        /// <summary>
        /// Authenticates the request as a user,
        /// with delegated permissions.
        /// </summary>
        /// <param name="req">The request to authenticate</param>
        public async Task AuthenticateRequestAsUserAsync(HttpRequestMessage req)
        {
            var tokenReq = new HttpRequestMessage(HttpMethod.Post, _settings.UserAuthentication.TokenUrl)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "password",
                    ["username"] = _settings.UserAuthentication.Username,
                    ["password"] = _settings.UserAuthentication.Password,
                    ["client_id"] = _settings.UserAuthentication.ClientId,
                    ["client_secret"] = _settings.UserAuthentication.ClientSecret,
                    ["scope"] = $"{_settings.ApiAppIdUri}/.default"
                })
            };

            var res = await _client.SendAsync(tokenReq);

            string json = await res.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);

            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
        }


        /// <summary>
        /// Authenticates the request as an app,
        /// with application permissions.
        /// </summary>
        /// <param name="req">The request to authenticate</param>
        public async Task AuthenticateRequestAsAppAsync(HttpRequestMessage req)
        {
            var scopes = new[] { $"{_settings.ApiAppIdUri}/.default" };
            var result = await _confidentialClientApp.AcquireTokenForClient(scopes).ExecuteAsync();
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
        }
    }
}

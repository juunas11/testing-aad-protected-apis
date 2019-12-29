using Joonasw.AadTestingDemo.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace Joonasw.AadTestingDemo.IntegrationTests.Utils
{
    public class AppFixture : IDisposable
    {
        private readonly WebApplicationFactory<Startup> _webAppFactory;

        public HttpClient Client { get; }
        public IntegrationTestSettings Settings { get; private set; }

        public AppFixture()
        {
            _webAppFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("IntegrationTesting");
                    builder.ConfigureAppConfiguration(configBuilder =>
                    {
                        // Adds user secrets for the integration test project
                        // Contains the Key Vault URL and API authentication settings for me
                        configBuilder.AddUserSecrets<AppFixture>();

                        // Build temporary config, get Key Vault URL, add Key Vault as config source
                        var config = configBuilder.Build();
                        string keyVaultUrl = config["IntegrationTest:KeyVaultUrl"];
                        if (!string.IsNullOrEmpty(keyVaultUrl))
                        {
                            // CI / CD pipeline sets up a credentials environment variable to use
                            var credentialsJson = Environment.GetEnvironmentVariable("AZURE_CREDENTIALS");
                            // If it is not present, we are running locally
                            if (string.IsNullOrEmpty(credentialsJson))
                            {
                                // Use local user authentication
                                configBuilder.AddAzureKeyVault(keyVaultUrl);
                            }
                            else
                            {
                                // Use credentials in JSON object
                                var credentials = (JObject)JsonConvert.DeserializeObject(credentialsJson);
                                var clientId = credentials?.Value<string>("clientId");
                                var clientSecret = credentials?.Value<string>("clientSecret");
                                configBuilder.AddAzureKeyVault(keyVaultUrl, clientId, clientSecret);
                            }

                            config = configBuilder.Build();
                        }

                        Settings = config.GetSection("IntegrationTest").Get<IntegrationTestSettings>();
                    });
                });
            Client = _webAppFactory.CreateDefaultClient();
        }

        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _webAppFactory.Dispose();
                }

                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
    }
}

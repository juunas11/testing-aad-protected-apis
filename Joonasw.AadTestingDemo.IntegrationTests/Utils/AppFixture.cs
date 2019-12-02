using Joonasw.AadTestingDemo.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
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
                            // This will use Managed Identity / local user authentication
                            // For this to work in a CI pipeline,
                            // you will need to somehow pass in a client id + client secret
                            // and use a different overload that takes those.
                            // Locally doing this is better though.
                            configBuilder.AddAzureKeyVault(keyVaultUrl);
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

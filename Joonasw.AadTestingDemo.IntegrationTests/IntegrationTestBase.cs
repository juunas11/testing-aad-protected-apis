using System.Net.Http;
using Joonasw.AadTestingDemo.IntegrationTests.Utils;
using Xunit;

namespace Joonasw.AadTestingDemo.IntegrationTests
{
    /// <summary>
    /// Base class for all integration tests
    /// </summary>
    [Collection("Integration")]
    public abstract class IntegrationTestBase
    {
        protected IntegrationTestBase(AppFixture app)
        {
            Client = app.Client;
            Settings = app.Settings;
            AccessTokenProvider = new AccessTokenProvider(app.Settings);
        }

        protected HttpClient Client { get; }
        protected IntegrationTestSettings Settings { get; }
        protected AccessTokenProvider AccessTokenProvider { get; }
    }
}

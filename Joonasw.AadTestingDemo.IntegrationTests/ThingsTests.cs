using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Joonasw.AadTestingDemo.IntegrationTests.Utils;
using Xunit;

namespace Joonasw.AadTestingDemo.IntegrationTests
{
    public class ThingsTests : IntegrationTestBase
    {
        public ThingsTests(AppFixture app) : base(app)
        {
        }

        [Theory]
        [InlineData("/Things")]
        [InlineData("/Things/other")]
        public async Task CallWithoutAuthenticationFails(string url)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, url);

            var res = await Client.SendAsync(req);

            Assert.Equal(HttpStatusCode.Unauthorized, res.StatusCode);
        }

        [Theory]
        [InlineData("/Things")]
        [InlineData("/Things/other")]
        public async Task CallWithUserAuthenticationSucceeds(string url)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            await AccessTokenProvider.AuthenticateRequestAsUserAsync(req);

            var res = await Client.SendAsync(req);

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }

        [Theory]
        [InlineData("/Things")]
        [InlineData("/Things/other")]
        public async Task CallWithAppAuthenticationSucceeds(string url)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            await AccessTokenProvider.AuthenticateRequestAsAppAsync(req);

            var res = await Client.SendAsync(req);

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
    }
}

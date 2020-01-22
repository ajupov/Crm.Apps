using System.Threading.Tasks;
using Crm.Apps.v1.Clients.OAuth.Clients;
using Xunit;

namespace Crm.Apps.Tests.Tests.OAuth
{
    public class OAuthTests
    {
        private readonly IOAuthClient _oAuthClient;

        public OAuthTests(IOAuthClient oAuthClient)
        {
            _oAuthClient = oAuthClient;
        }

        [Fact]
        public async Task WhenGetTokens_ThenSuccess()
        {
            var tokens = await _oAuthClient.GetTokensAsync("au073", "17101994");

            Assert.NotNull(tokens.AccessToken);
            Assert.NotNull(tokens.RefreshToken);
            Assert.True(tokens.ExpiresIn > 0);
            Assert.NotNull(tokens.TokenType);
        }
    }
}
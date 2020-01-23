using System.Threading.Tasks;
using Crm.Apps.Tests.Settings;
using Crm.Apps.v1.Clients.OAuth.Clients;
using Microsoft.Extensions.Options;
using Xunit;

namespace Crm.Apps.Tests.Tests.OAuth
{
    public class OAuthTests
    {
        private readonly OAuthSettings _oauthSettings;
        private readonly IOAuthClient _oauthClient;

        public OAuthTests(IOptions<OAuthSettings> options, IOAuthClient oauthClient)
        {
            _oauthSettings = options.Value;
            _oauthClient = oauthClient;
        }

        [Fact]
        public async Task WhenGetTokens_ThenSuccess()
        {
            var tokens = await _oauthClient.GetTokensAsync(_oauthSettings.Username, _oauthSettings.Password);

            Assert.NotNull(tokens.AccessToken);
            Assert.NotNull(tokens.RefreshToken);
            Assert.True(tokens.ExpiresIn > 0);
            Assert.NotNull(tokens.TokenType);
        }
    }
}
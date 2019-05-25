using System;
using System.Threading.Tasks;
using Crm.Apps.Tests.Dsl.Creator;
using Crm.Clients.Identities.Clients.IdentityTokens;
using Crm.Clients.Identities.Models;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Identities
{
    public class IdentityTokensTests
    {
        private readonly ICreate _create;
        private readonly IIdentityTokensClient _identityTokensClient;

        public IdentityTokensTests(ICreate create, IIdentityTokensClient identityTokensClient)
        {
            _create = create;
            _identityTokensClient = identityTokensClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var identity = await _create.Identity.WithUserId(user.Id).BuildAsync().ConfigureAwait(false);
            var token = await _create.IdentityToken.WithIdentityId(identity.Id).BuildAsync().ConfigureAwait(false);

            var createdToken =
                await _identityTokensClient.GetAsync(token.IdentityId, token.Value).ConfigureAwait(false);

            Assert.NotNull(createdToken);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var identity = await _create.Identity.WithUserId(user.Id).BuildAsync().ConfigureAwait(false);
            var token = new IdentityToken
            {
                IdentityId = identity.Id,
                Value = "Test",
                ExpirationDateTime = DateTime.Now.AddDays(1)
            };

            var identityId = await _identityTokensClient.CreateAsync(token).ConfigureAwait(false);

            var createdToken =
                await _identityTokensClient.GetAsync(token.IdentityId, token.Value).ConfigureAwait(false);

            Assert.NotNull(createdToken);
            Assert.Equal(identityId, createdToken.Id);
            Assert.Equal(token.Value, createdToken.Value);
            Assert.True(createdToken.CreateDateTime.IsMoreThanMinValue());
            Assert.Equal(token.ExpirationDateTime, createdToken.ExpirationDateTime);
            Assert.Null(createdToken.UseDateTime);
        }

        [Fact]
        public async Task WhenSetIsUsed_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var identity = await _create.Identity.WithUserId(user.Id).BuildAsync().ConfigureAwait(false);
            var token = await _create.IdentityToken.WithIdentityId(identity.Id).BuildAsync().ConfigureAwait(false);

            await _identityTokensClient.SetIsUsedAsync(token.Id).ConfigureAwait(false);

            var usedToken = await _identityTokensClient.GetAsync(token.IdentityId, token.Value).ConfigureAwait(false);

            Assert.True(usedToken.UseDateTime.HasValue);
            Assert.True(usedToken.UseDateTime.Value <= DateTime.Now);
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Dsl.Creator;
using Crm.Clients.Identities.Clients.Identities;
using Crm.Clients.Identities.Models;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Identities
{
    public class IdentitiesTests
    {
        private readonly ICreate _create;
        private readonly IIdentitiesClient _identitiesClient;

        public IdentitiesTests(ICreate create, IIdentitiesClient identitiesClient)
        {
            _create = create;
            _identitiesClient = identitiesClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var types = await _identitiesClient.GetTypesAsync().ConfigureAwait(false);

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var identityId = (await _create.Identity.WithUserId(user.Id).BuildAsync().ConfigureAwait(false)).Id;

            var createdIdentity = await _identitiesClient.GetAsync(identityId).ConfigureAwait(false);

            Assert.NotNull(createdIdentity);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);

            var identityIds = (await Task.WhenAll(
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Vkontakte).WithKey("Test1").BuildAsync(),
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Instagram).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            var identities = await _identitiesClient.GetListAsync(identityIds).ConfigureAwait(false);

            Assert.NotEmpty(identities);
            Assert.Equal(identityIds.Count, identities.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);

            await Task.WhenAll(
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Vkontakte).WithKey("Test1").BuildAsync(),
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Instagram).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false);

            var identities = await _identitiesClient.GetPagedListAsync(sortBy: "CreateDateTime", orderBy: "desc")
                .ConfigureAwait(false);
            var results = identities.Skip(1).Zip(identities,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(identities);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);

            var identity = new Identity
            {
                UserId = user.Id,
                Type = IdentityType.None,
                Key = "Test",
                PasswordHash = string.Empty,
                IsPrimary = true,
                IsVerified = true
            };

            var identityId = await _identitiesClient.CreateAsync(identity).ConfigureAwait(false);

            var createdIdentity = await _identitiesClient.GetAsync(identityId).ConfigureAwait(false);

            Assert.NotNull(createdIdentity);
            Assert.Equal(identityId, createdIdentity.Id);
            Assert.True(createdIdentity.IsPrimary);
            Assert.True(createdIdentity.IsVerified);
            Assert.True(createdIdentity.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var identity = await _create.Identity.WithUserId(user.Id).BuildAsync().ConfigureAwait(false);

            identity.Type = IdentityType.Vkontakte;
            identity.Key = "Test2";
            identity.PasswordHash = "Test2";
            identity.IsPrimary = true;
            identity.IsPrimary = true;

            await _identitiesClient.UpdateAsync(identity).ConfigureAwait(false);

            var updatedIdentity = await _identitiesClient.GetAsync(identity.Id).ConfigureAwait(false);

            Assert.Equal(identity.Type, updatedIdentity.Type);
            Assert.Equal(identity.Key, updatedIdentity.Key);
            Assert.Equal(identity.PasswordHash, updatedIdentity.PasswordHash);
            Assert.Equal(identity.IsPrimary, updatedIdentity.IsPrimary);
        }

        [Fact]
        public async Task WhenVerify_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);

            var identityIds = (await Task.WhenAll(
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Vkontakte).WithKey("Test1").BuildAsync(),
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Instagram).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _identitiesClient.VerifyAsync(identityIds).ConfigureAwait(false);

            var identities = await _identitiesClient.GetListAsync(identityIds).ConfigureAwait(false);

            Assert.All(identities, x => Assert.True(x.IsVerified));
        }

        [Fact]
        public async Task WhenUnverify_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);

            var identityIds = (await Task.WhenAll(
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Vkontakte).WithKey("Test1").BuildAsync(),
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Instagram).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _identitiesClient.UnverifyAsync(identityIds).ConfigureAwait(false);

            var identities = await _identitiesClient.GetListAsync(identityIds).ConfigureAwait(false);

            Assert.All(identities, x => Assert.False(x.IsVerified));
        }

        [Fact]
        public async Task WhenSetAsPrimary_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);

            var identityIds = (await Task.WhenAll(
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Vkontakte).WithKey("Test1").BuildAsync(),
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Instagram).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _identitiesClient.SetAsPrimaryAsync(identityIds).ConfigureAwait(false);

            var identities = await _identitiesClient.GetListAsync(identityIds).ConfigureAwait(false);

            Assert.All(identities, x => Assert.True(x.IsPrimary));
        }

        [Fact]
        public async Task WhenResetAsPrimary_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var user = await _create.User.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);

            var identityIds = (await Task.WhenAll(
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Vkontakte).WithKey("Test1").BuildAsync(),
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Instagram).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _identitiesClient.ResetAsPrimaryAsync(identityIds).ConfigureAwait(false);

            var identities = await _identitiesClient.GetListAsync(identityIds).ConfigureAwait(false);

            Assert.All(identities, x => Assert.False(x.IsPrimary));
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Identities.Clients;
using Crm.Clients.Identities.Models;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Tests.Identities
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
            var types = await _identitiesClient.GetTypesAsync();

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var user = await _create.User.WithAccountId(account.Id).BuildAsync();
            var identityId = (await _create.Identity.WithUserId(user.Id).BuildAsync()).Id;

            var createdIdentity = await _identitiesClient.GetAsync(identityId);

            Assert.NotNull(createdIdentity);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var user = await _create.User.WithAccountId(account.Id).BuildAsync();

            var identityIds = (await Task.WhenAll(
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Vkontakte).WithKey("Test1").BuildAsync(),
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Instagram).WithKey("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            var identities = await _identitiesClient.GetListAsync(identityIds);

            Assert.NotEmpty(identities);
            Assert.Equal(identityIds.Count, identities.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var user = await _create.User.WithAccountId(account.Id).BuildAsync();

            await Task.WhenAll(
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Vkontakte).WithKey("Test1").BuildAsync(),
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Instagram).WithKey("Test2").BuildAsync())
                ;

            var identities = await _identitiesClient.GetPagedListAsync(sortBy: "CreateDateTime", orderBy: "desc")
                ;
            var results = identities.Skip(1).Zip(identities,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(identities);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var user = await _create.User.WithAccountId(account.Id).BuildAsync();

            var identity = new Identity
            {
                UserId = user.Id,
                Type = IdentityType.None,
                Key = "Test",
                IsPrimary = true,
                IsVerified = true
            };

            var identityId = await _identitiesClient.CreateAsync(identity);

            var createdIdentity = await _identitiesClient.GetAsync(identityId);

            Assert.NotNull(createdIdentity);
            Assert.Equal(identityId, createdIdentity.Id);
            Assert.True(createdIdentity.IsPrimary);
            Assert.True(createdIdentity.IsVerified);
            Assert.True(createdIdentity.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var user = await _create.User.WithAccountId(account.Id).BuildAsync();
            var identity = await _create.Identity.WithUserId(user.Id).BuildAsync();

            identity.Type = IdentityType.Vkontakte;
            identity.Key = "Test2";
            identity.IsPrimary = true;
            identity.IsPrimary = true;

            await _identitiesClient.UpdateAsync(identity);

            var updatedIdentity = await _identitiesClient.GetAsync(identity.Id);

            Assert.Equal(identity.Type, updatedIdentity.Type);
            Assert.Equal(identity.Key, updatedIdentity.Key);
            Assert.Equal(identity.IsPrimary, updatedIdentity.IsPrimary);
        }

        [Fact]
        public async Task WhenSetPassword_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var user = await _create.User.WithAccountId(account.Id).BuildAsync();
            var identity = await _create.Identity.WithUserId(user.Id).BuildAsync();

            await _identitiesClient.SetPasswordAsync(identity.Id, "Test");
        }

        [Fact]
        public async Task WhenIsPasswordCorrect_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var user = await _create.User.WithAccountId(account.Id).BuildAsync();
            await _create.Identity.WithUserId(user.Id).WithKey("Test").WithPassword("Test").BuildAsync();

            var result = await _identitiesClient.IsPasswordCorrectAsync("Test", "Test");

            Assert.True(result);
        }

        [Fact]
        public async Task WhenVerify_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var user = await _create.User.WithAccountId(account.Id).BuildAsync();

            var identityIds = (await Task.WhenAll(
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Vkontakte).WithKey("Test1").BuildAsync(),
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Instagram).WithKey("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _identitiesClient.VerifyAsync(identityIds);

            var identities = await _identitiesClient.GetListAsync(identityIds);

            Assert.All(identities, x => Assert.True(x.IsVerified));
        }

        [Fact]
        public async Task WhenUnverify_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var user = await _create.User.WithAccountId(account.Id).BuildAsync();

            var identityIds = (await Task.WhenAll(
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Vkontakte).WithKey("Test1").BuildAsync(),
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Instagram).WithKey("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _identitiesClient.UnverifyAsync(identityIds);

            var identities = await _identitiesClient.GetListAsync(identityIds);

            Assert.All(identities, x => Assert.False(x.IsVerified));
        }

        [Fact]
        public async Task WhenSetAsPrimary_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var user = await _create.User.WithAccountId(account.Id).BuildAsync();

            var identityIds = (await Task.WhenAll(
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Vkontakte).WithKey("Test1").BuildAsync(),
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Instagram).WithKey("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _identitiesClient.SetAsPrimaryAsync(identityIds);

            var identities = await _identitiesClient.GetListAsync(identityIds);

            Assert.All(identities, x => Assert.True(x.IsPrimary));
        }

        [Fact]
        public async Task WhenResetAsPrimary_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var user = await _create.User.WithAccountId(account.Id).BuildAsync();

            var identityIds = (await Task.WhenAll(
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Vkontakte).WithKey("Test1").BuildAsync(),
                    _create.Identity.WithUserId(user.Id).WithType(IdentityType.Instagram).WithKey("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _identitiesClient.ResetAsPrimaryAsync(identityIds);

            var identities = await _identitiesClient.GetListAsync(identityIds);

            Assert.All(identities, x => Assert.False(x.IsPrimary));
        }
    }
}
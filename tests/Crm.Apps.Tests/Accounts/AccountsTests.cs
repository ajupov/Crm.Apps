using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Dsl;
using Crm.Clients.Accounts.Clients.Accounts;
using Crm.Clients.Accounts.Models;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Accounts
{
    public class AccountsTests
    {
        private readonly IAccountsClient _accountsClient;

        public AccountsTests(IAccountsClient accountsClient)
        {
            _accountsClient = accountsClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = Create.Account().Build();

            var createdAccountId = await _accountsClient.CreateAsync(account).ConfigureAwait(false);
            var createdAccount = await _accountsClient.GetAsync(createdAccountId).ConfigureAwait(false);

            Assert.NotNull(createdAccount);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var createdAccountsIds = await Task.WhenAll(
                    _accountsClient.CreateAsync(Create.Account().Build()),
                    _accountsClient.CreateAsync(Create.Account().Build()))
                .ConfigureAwait(false);

            var createdAccounts = await _accountsClient.GetListAsync(createdAccountsIds).ConfigureAwait(false);

            Assert.NotEmpty(createdAccounts);
            Assert.Equal(createdAccountsIds.Length, createdAccounts.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            await Task.WhenAll(
                    _accountsClient.CreateAsync(Create.Account().Build()),
                    _accountsClient.CreateAsync(Create.Account().Build()))
                .ConfigureAwait(false);

            var anyAccounts = await _accountsClient.GetPagedListAsync(sortBy: "CreateDateTime", orderBy: "desc")
                .ConfigureAwait(false);

            var results = anyAccounts.Skip(1).Zip(anyAccounts,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(anyAccounts);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = Create.Account().WithSetting("test").Build();

            var createdAccountId = await _accountsClient.CreateAsync(account).ConfigureAwait(false);
            var createdAccount = await _accountsClient.GetAsync(createdAccountId).ConfigureAwait(false);
            var createdAccountSettings = createdAccount.Settings.Select(x => x.Value);

            Assert.NotNull(createdAccount);
            Assert.Equal(createdAccountId, createdAccount.Id);
            Assert.False(createdAccount.IsLocked);
            Assert.False(createdAccount.IsDeleted);
            Assert.True(createdAccount.CreateDateTime.IsMoreThanMinValue());
            Assert.NotEmpty(createdAccountSettings);
            Assert.Equal(account.Settings.Single().Value, createdAccount.Settings.Single().Value);
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = Create.Account().WithSetting("test").Build();
            var createdAccountId = await _accountsClient.CreateAsync(account).ConfigureAwait(false);
            var createdAccount = await _accountsClient.GetAsync(createdAccountId).ConfigureAwait(false);

            createdAccount.IsLocked = true;
            createdAccount.IsDeleted = true;
            createdAccount.Settings.Add(new AccountSetting {Type = AccountSettingType.None, Value = "test"});

            await _accountsClient.UpdateAsync(createdAccount).ConfigureAwait(false);
            var updatedAccount = await _accountsClient.GetAsync(createdAccountId).ConfigureAwait(false);

            Assert.Equal(createdAccount.IsLocked, updatedAccount.IsLocked);
            Assert.Equal(createdAccount.IsDeleted, updatedAccount.IsDeleted);
            Assert.Equal(createdAccount.Settings.Single().Value, updatedAccount.Settings.Single().Value);
        }

        [Fact]
        public async Task WhenLock_ThenSuccess()
        {
            var createdAccountsIds = await Task.WhenAll(
                    _accountsClient.CreateAsync(Create.Account().Build()),
                    _accountsClient.CreateAsync(Create.Account().Build()))
                .ConfigureAwait(false);

            await _accountsClient.LockAsync(createdAccountsIds).ConfigureAwait(false);
            var lockedAccounts = await _accountsClient.GetListAsync(createdAccountsIds).ConfigureAwait(false);

            Assert.All(lockedAccounts, x => Assert.True(x.IsLocked));
        }

        [Fact]
        public async Task WhenUnlock_ThenSuccess()
        {
            var createdAccountsIds = await Task.WhenAll(
                    _accountsClient.CreateAsync(Create.Account().Build()),
                    _accountsClient.CreateAsync(Create.Account().Build()))
                .ConfigureAwait(false);

            await _accountsClient.UnlockAsync(createdAccountsIds).ConfigureAwait(false);
            var unlockedAccounts = await _accountsClient.GetListAsync(createdAccountsIds).ConfigureAwait(false);

            Assert.All(unlockedAccounts, x => Assert.False(x.IsLocked));
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var createdAccountsIds = await Task.WhenAll(
                    _accountsClient.CreateAsync(Create.Account().Build()),
                    _accountsClient.CreateAsync(Create.Account().Build()))
                .ConfigureAwait(false);

            await _accountsClient.DeleteAsync(createdAccountsIds).ConfigureAwait(false);
            var deletedAccounts = await _accountsClient.GetListAsync(createdAccountsIds).ConfigureAwait(false);

            Assert.All(deletedAccounts, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var createdAccountsIds = await Task.WhenAll(
                    _accountsClient.CreateAsync(Create.Account().Build()),
                    _accountsClient.CreateAsync(Create.Account().Build()))
                .ConfigureAwait(false);

            await _accountsClient.RestoreAsync(createdAccountsIds).ConfigureAwait(false);
            var restoredAccounts = await _accountsClient.GetListAsync(createdAccountsIds).ConfigureAwait(false);

            Assert.All(restoredAccounts, x => Assert.False(x.IsDeleted));
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
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
            var id = await _accountsClient.CreateAsync().ConfigureAwait(false);

            var account = await _accountsClient.GetAsync(id).ConfigureAwait(false);

            Assert.NotNull(account);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var ids = await Task.WhenAll(_accountsClient.CreateAsync(), _accountsClient.CreateAsync())
                .ConfigureAwait(false);

            var accounts = await _accountsClient.GetListAsync(ids).ConfigureAwait(false);

            Assert.NotEmpty(accounts);
            Assert.Equal(ids.Length, accounts.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            await Task.WhenAll(_accountsClient.CreateAsync(), _accountsClient.CreateAsync()).ConfigureAwait(false);

            var accounts = await _accountsClient
                .GetPagedListAsync(sortBy: nameof(Account.CreateDateTime), orderBy: "desc")
                .ConfigureAwait(false);

            var results = accounts.Skip(1).Zip(accounts,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(accounts);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var id = await _accountsClient.CreateAsync().ConfigureAwait(false);

            var account = await _accountsClient.GetAsync(id).ConfigureAwait(false);

            Assert.NotNull(account);
            Assert.Equal(id, account.Id);
            Assert.False(account.IsLocked);
            Assert.False(account.IsDeleted);
            Assert.True(account.CreateDateTime.IsMoreThanMinValue());
            Assert.Empty(account.Settings);
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var id = await _accountsClient.CreateAsync().ConfigureAwait(false);
            var createdAccount = await _accountsClient.GetAsync(id).ConfigureAwait(false);

            createdAccount.IsLocked = true;
            createdAccount.IsDeleted = true;
            createdAccount.Settings.Add(new AccountSetting {Type = AccountSettingType.None, Value = "Test"});
            var createdAccountSettings = createdAccount.Settings.Select(x => new {x.Type, x.Value});

            await _accountsClient.UpdateAsync(createdAccount).ConfigureAwait(false);

            var updatedAccount = await _accountsClient.GetAsync(id).ConfigureAwait(false);
            var updatedAccountSettings = updatedAccount.Settings.Select(x => new {x.Type, x.Value});

            Assert.Equal(createdAccount.IsLocked, updatedAccount.IsLocked);
            Assert.Equal(createdAccount.IsDeleted, updatedAccount.IsDeleted);
            Assert.Equal(createdAccountSettings, updatedAccountSettings);
        }

        [Fact]
        public async Task WhenLock_ThenSuccess()
        {
            var ids = await Task.WhenAll(_accountsClient.CreateAsync(), _accountsClient.CreateAsync())
                .ConfigureAwait(false);

            await _accountsClient.LockAsync(ids).ConfigureAwait(false);

            var lockedAccounts = await _accountsClient.GetListAsync(ids).ConfigureAwait(false);

            Assert.True(lockedAccounts.All(x => x.IsLocked));
        }

        [Fact]
        public async Task WhenUnlock_ThenSuccess()
        {
            var ids = await Task.WhenAll(_accountsClient.CreateAsync(), _accountsClient.CreateAsync())
                .ConfigureAwait(false);

            await _accountsClient.UnlockAsync(ids).ConfigureAwait(false);

            var unlockedAccounts = await _accountsClient.GetListAsync(ids).ConfigureAwait(false);

            Assert.True(unlockedAccounts.All(x => !x.IsLocked));
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var ids = await Task.WhenAll(_accountsClient.CreateAsync(), _accountsClient.CreateAsync())
                .ConfigureAwait(false);

            await _accountsClient.DeleteAsync(ids).ConfigureAwait(false);

            var deletedAccounts = await _accountsClient.GetListAsync(ids).ConfigureAwait(false);

            Assert.True(deletedAccounts.All(x => x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var ids = await Task.WhenAll(_accountsClient.CreateAsync(), _accountsClient.CreateAsync())
                .ConfigureAwait(false);

            await _accountsClient.RestoreAsync(ids).ConfigureAwait(false);

            var restoredAccounts = await _accountsClient.GetListAsync(ids).ConfigureAwait(false);

            Assert.True(restoredAccounts.All(x => !x.IsDeleted));
        }
    }
}
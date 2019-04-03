using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Clients.Accounts;
using Crm.Clients.Accounts.Clients.AccountsDefault;
using Crm.Clients.Accounts.Clients.AccountSettings;
using Crm.Clients.Accounts.Models;
using Xunit;

namespace Crm.Apps.Tests.Accounts
{
    public class AccountsTests
    {
        private readonly IAccountsDefaultClient _accountsDefaultClient;
        private readonly IAccountsClient _accountsClient;
        private readonly IAccountsSettingsClient _accountsSettingsClient;

        public AccountsTests(
            IAccountsDefaultClient accountsDefaultClient,
            IAccountsClient accountsClient,
            IAccountsSettingsClient accountsSettingsClient)
        {
            _accountsDefaultClient = accountsDefaultClient;
            _accountsClient = accountsClient;
            _accountsSettingsClient = accountsSettingsClient;
        }

        [Fact]
        public Task Status()
        {
            return _accountsDefaultClient.StatusAsync();
        }

        [Fact]
        public async Task GetAccountSettingsTypes()
        {
            var types = await _accountsSettingsClient.GetTypesAsync().ConfigureAwait(false);

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task GetAccount()
        {
            var id = await _accountsClient.CreateAsync().ConfigureAwait(false);
            var account = await _accountsClient.GetAsync(id).ConfigureAwait(false);

            Assert.NotNull(account);
            Assert.Equal(id, account.Id);
        }

        [Fact]
        public async Task GetAccountsList()
        {
            var ids = await Task.WhenAll(_accountsClient.CreateAsync(), _accountsClient.CreateAsync())
                .ConfigureAwait(false);
            var accounts = await _accountsClient.GetListAsync(ids).ConfigureAwait(false);

            Assert.NotEmpty(accounts);
            Assert.Equal(ids.Length, accounts.Count);
        }

        [Fact]
        public async Task GetPagedList()
        {
            await Task.WhenAll(_accountsClient.CreateAsync(), _accountsClient.CreateAsync()).ConfigureAwait(false);
            var accounts = await _accountsClient.GetPagedListAsync(sortBy: "CreateDateTime", orderBy: "desc")
                .ConfigureAwait(false);

            Assert.NotEmpty(accounts);

            var results = accounts.Skip(1).Zip(accounts,
                (current, previous) => current.CreateDateTime > previous.CreateDateTime);

            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task CreateAccount()
        {
            var id = await _accountsClient.CreateAsync().ConfigureAwait(false);

            Assert.NotEqual(id, Guid.Empty);
        }

        [Fact]
        public async Task UpdateAccount()
        {
            var id = await _accountsClient.CreateAsync().ConfigureAwait(false);
            var account = await _accountsClient.GetAsync(id).ConfigureAwait(false);

            account.IsLocked = true;
            account.IsDeleted = true;
            account.Settings = new List<AccountSetting>
            {
                new AccountSetting
                {
                    Type = AccountSettingType.None,
                    Value = "Test"
                }
            };

            await _accountsClient.UpdateAsync(account).ConfigureAwait(false);
            var updatedAccount = await _accountsClient.GetAsync(id).ConfigureAwait(false);

            Assert.Equal(account.IsLocked, updatedAccount.IsLocked);
            Assert.Equal(account.IsDeleted, updatedAccount.IsDeleted);
            Assert.Equal(account.Settings, updatedAccount.Settings);
        }

        [Fact]
        public async Task LockAccount()
        {
            var id = await _accountsClient.CreateAsync().ConfigureAwait(false);
            await _accountsClient.LockAsync(new List<Guid> {id}).ConfigureAwait(false);
            var lockedAccount = await _accountsClient.GetAsync(id).ConfigureAwait(false);

            Assert.True(lockedAccount.IsLocked);
        }

        [Fact]
        public async Task UnlockAccount()
        {
            var id = await _accountsClient.CreateAsync().ConfigureAwait(false);
            await _accountsClient.UnlockAsync(new List<Guid> {id}).ConfigureAwait(false);
            var unlockedAccount = await _accountsClient.GetAsync(id).ConfigureAwait(false);

            Assert.False(unlockedAccount.IsLocked);
        }

        [Fact]
        public async Task DeleteAccount()
        {
            var id = await _accountsClient.CreateAsync().ConfigureAwait(false);
            await _accountsClient.DeleteAsync(new List<Guid> {id}).ConfigureAwait(false);
            var deletedAccount = await _accountsClient.GetAsync(id).ConfigureAwait(false);

            Assert.True(deletedAccount.IsDeleted);
        }

        [Fact]
        public async Task RestoreAccount()
        {
            var id = await _accountsClient.CreateAsync().ConfigureAwait(false);
            await _accountsClient.RestoreAsync(new List<Guid> {id}).ConfigureAwait(false);
            var restoredAccount = await _accountsClient.GetAsync(id).ConfigureAwait(false);

            Assert.False(restoredAccount.IsDeleted);
        }
    }
}
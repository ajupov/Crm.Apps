using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Accounts.Clients;
using Crm.Clients.Accounts.Models;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Tests.Accounts
{
    public class AccountsTests
    {
        private readonly ICreate _create;
        private readonly IAccountsClient _accountsClient;

        public AccountsTests(
            ICreate create,
            IAccountsClient accountsClient)
        {
            _create = create;
            _accountsClient = accountsClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var types = await _accountsClient.GetTypesAsync();

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account
                .BuildAsync();

            var createdAccount = await _accountsClient.GetAsync(account.Id);

            Assert.NotNull(createdAccount);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var accountIds = (await Task.WhenAll(
                    _create.Account.BuildAsync(),
                    _create.Account.BuildAsync()))
                .Select(x => x.Id)
                .ToArray();

            var accounts = await _accountsClient.GetListAsync(accountIds);

            Assert.NotEmpty(accounts);

            Assert.Equal(accountIds.Length, accounts.Length);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            await Task.WhenAll(
                _create.Account.BuildAsync(),
                _create.Account.BuildAsync());

            var accounts = await _accountsClient.GetPagedListAsync();

            var results = accounts
                .Skip(1)
                .Zip(accounts, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(accounts);

            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = new Account(AccountType.MlmSystem, new List<AccountSetting>
            {
                new AccountSetting(AccountSettingType.PartnersEnabled, "Test")
            });

            var accountId = await _accountsClient.CreateAsync(account);

            var createdAccount = await _accountsClient.GetAsync(accountId);

            Assert.NotNull(createdAccount);

            Assert.False(createdAccount.IsLocked);
            Assert.False(createdAccount.IsDeleted);
            Assert.True(createdAccount.CreateDateTime.IsMoreThanMinValue());

            Assert.NotEmpty(createdAccount.Settings.Select(x => x.Value));

            Assert.Equal(account.Settings.Single().Type, createdAccount.Settings.Single().Type);
            Assert.Equal(account.Settings.Single().Value, createdAccount.Settings.Single().Value);
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account
                .BuildAsync();

            account.IsLocked = true;
            account.IsDeleted = true;
            account.Settings.Add(new AccountSetting(AccountSettingType.PartnersEnabled, "Test"));

            await _accountsClient.UpdateAsync(account);

            var updatedAccount = await _accountsClient.GetAsync(account.Id);

            Assert.Equal(account.IsLocked, updatedAccount.IsLocked);
            Assert.Equal(account.IsDeleted, updatedAccount.IsDeleted);
            Assert.Equal(account.Settings.Single().Type, updatedAccount.Settings.Single().Type);
            Assert.Equal(account.Settings.Single().Value, updatedAccount.Settings.Single().Value);
        }

        [Fact]
        public async Task WhenLock_ThenSuccess()
        {
            var accountIds = (await Task.WhenAll(
                    _create.Account.BuildAsync(),
                    _create.Account.BuildAsync()))
                .Select(x => x.Id)
                .ToArray();

            await _accountsClient.LockAsync(accountIds);

            var accounts = await _accountsClient.GetListAsync(accountIds);

            Assert.All(accounts, x => Assert.True(x.IsLocked));
        }

        [Fact]
        public async Task WhenUnlock_ThenSuccess()
        {
            var accountIds = (await Task.WhenAll(
                    _create.Account.BuildAsync(),
                    _create.Account.BuildAsync()))
                .Select(x => x.Id)
                .ToArray();

            await _accountsClient.UnlockAsync(accountIds);

            var accounts = await _accountsClient.GetListAsync(accountIds);

            Assert.All(accounts, x => Assert.False(x.IsLocked));
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var accountIds = (await Task.WhenAll(
                    _create.Account.BuildAsync(),
                    _create.Account.BuildAsync()))
                .Select(x => x.Id)
                .ToArray();

            await _accountsClient.DeleteAsync(accountIds);

            var accounts = await _accountsClient.GetListAsync(accountIds);

            Assert.All(accounts, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var accountIds = (await Task.WhenAll(
                    _create.Account.BuildAsync(),
                    _create.Account.BuildAsync()))
                .Select(x => x.Id)
                .ToArray();

            await _accountsClient.RestoreAsync(accountIds);

            var accounts = await _accountsClient.GetListAsync(accountIds);

            Assert.All(accounts, x => Assert.False(x.IsDeleted));
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Accounts.Clients;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Accounts.RequestParameters;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Tests.Accounts
{
    public class AccountsTests
    {
        private readonly ICreate _create;
        private readonly IAccountsClient _accountsClient;

        public AccountsTests(ICreate create, IAccountsClient accountsClient)
        {
            _create = create;
            _accountsClient = accountsClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var actualTypes = await _accountsClient.GetTypesAsync();

            Assert.NotEmpty(actualTypes);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();

            var actualAccount = await _accountsClient.GetAsync(account.Id);

            Assert.NotNull(actualAccount);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var accountIds = (await Task.WhenAll(
                    _create.Account.BuildAsync(),
                    _create.Account.BuildAsync()))
                .Select(x => x.Id)
                .ToArray();

            var actualAccounts = await _accountsClient.GetListAsync(accountIds);

            Assert.NotEmpty(actualAccounts);
            Assert.Equal(accountIds.Length, actualAccounts.Length);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            await Task.WhenAll(
                _create.Account.BuildAsync(),
                _create.Account.BuildAsync());

            var request = new AccountGetPagedListParameter();
            var actualAccounts = await _accountsClient.GetPagedListAsync(request);

            var results = actualAccounts
                .Skip(1)
                .Zip(actualAccounts, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(actualAccounts);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var request = new AccountCreateRequest
            {
                Type = AccountType.MlmSystem,
                IsLocked = false,
                IsDeleted = false,
                Settings = new List<AccountSetting>
                {
                    new AccountSetting
                    {
                        Type = AccountSettingType.PartnersEnabled,
                        Value = "Test"
                    }
                }
            };

            var accountId = await _accountsClient.CreateAsync(request);

            var actualAccount = await _accountsClient.GetAsync(accountId);

            Assert.NotNull(actualAccount);
            Assert.False(actualAccount.IsLocked);
            Assert.False(actualAccount.IsDeleted);
            Assert.True(actualAccount.CreateDateTime.IsMoreThanMinValue());
            Assert.NotEmpty(actualAccount.Settings.Select(x => x.Value));
            Assert.Equal(request.Settings.Single().Type, actualAccount.Settings.Single().Type);
            Assert.Equal(request.Settings.Single().Value, actualAccount.Settings.Single().Value);
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var request = new AccountUpdateRequest
            {
                Id = account.Id,
                Type = AccountType.MlmSystem,
                IsLocked = true,
                IsDeleted = true,
                Settings = new List<AccountSetting>
                {
                    new AccountSetting
                    {
                        Type = AccountSettingType.PartnersEnabled,
                        Value = "Test"
                    }
                }
            };

            await _accountsClient.UpdateAsync(request);

            var actualAccount = await _accountsClient.GetAsync(account.Id);

            Assert.Equal(request.Type, actualAccount.Type);
            Assert.Equal(request.IsLocked, actualAccount.IsLocked);
            Assert.Equal(request.IsDeleted, actualAccount.IsDeleted);
            Assert.Equal(request.Settings.Single().Type, actualAccount.Settings.Single().Type);
            Assert.Equal(request.Settings.Single().Value, actualAccount.Settings.Single().Value);
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

            var actualAccounts = await _accountsClient.GetListAsync(accountIds);

            Assert.All(actualAccounts, x => Assert.True(x.IsLocked));
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

            var actualAccounts = await _accountsClient.GetListAsync(accountIds);

            Assert.All(actualAccounts, x => Assert.False(x.IsLocked));
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

            var actualAccounts = await _accountsClient.GetListAsync(accountIds);

            Assert.All(actualAccounts, x => Assert.True(x.IsDeleted));
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

            var actualAccounts = await _accountsClient.GetListAsync(accountIds);

            Assert.All(actualAccounts, x => Assert.False(x.IsDeleted));
        }
    }
}
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

        public AccountsTests(ICreate create, IAccountsClient accountsClient)
        {
            _create = create;
            _accountsClient = accountsClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);

            var createdAccount = await _accountsClient.GetAsync(account.Id).ConfigureAwait(false);

            Assert.NotNull(createdAccount);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var accountIds = (await Task.WhenAll(_create.Account.BuildAsync(), _create.Account.BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            var accounts = await _accountsClient.GetListAsync(accountIds).ConfigureAwait(false);

            Assert.NotEmpty(accounts);
            Assert.Equal(accountIds.Count, accounts.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            await Task.WhenAll(_create.Account.BuildAsync(), _create.Account.BuildAsync()).ConfigureAwait(false);

            var accounts = await _accountsClient.GetPagedListAsync(sortBy: "CreateDateTime", orderBy: "desc")
                .ConfigureAwait(false);
            var results = accounts.Skip(1).Zip(accounts,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(accounts);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = new Account
            {
                Settings = new List<AccountSetting>
                {
                    new AccountSetting
                    {
                        Type = AccountSettingType.None,
                        Value = "Test"
                    }
                }
            };
            
            var accountId = await _accountsClient.CreateAsync(account).ConfigureAwait(false);

            var createdAccount = await _accountsClient.GetAsync(accountId).ConfigureAwait(false);
            var createdAccountSettings = createdAccount.Settings.Select(x => x.Value);

            Assert.NotNull(createdAccount);
            Assert.Equal(accountId, createdAccount.Id);
            Assert.False(createdAccount.IsLocked);
            Assert.False(createdAccount.IsDeleted);
            Assert.True(createdAccount.CreateDateTime.IsMoreThanMinValue());
            Assert.NotEmpty(createdAccountSettings);
            Assert.Equal(new Account
            {
                Settings = new List<AccountSetting>
                {
                    new AccountSetting
                    {
                        Type = AccountSettingType.None,
                        Value = "Test"
                    }
                }
            }.Settings.Single().Value, createdAccount.Settings.Single().Value);
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            account.IsLocked = true;
            account.IsDeleted = true;
            account.Settings.Add(new AccountSetting {Type = AccountSettingType.None, Value = "Test"});

            await _accountsClient.UpdateAsync(account).ConfigureAwait(false);

            var updatedAccount = await _accountsClient.GetAsync(account.Id).ConfigureAwait(false);

            Assert.Equal(account.IsLocked, updatedAccount.IsLocked);
            Assert.Equal(account.IsDeleted, updatedAccount.IsDeleted);
            Assert.Equal(account.Settings.Single().Value, updatedAccount.Settings.Single().Value);
        }

        [Fact]
        public async Task WhenLock_ThenSuccess()
        {
            var accountIds = (await Task.WhenAll(_create.Account.BuildAsync(), _create.Account.BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _accountsClient.LockAsync(accountIds).ConfigureAwait(false);

            var accounts = await _accountsClient.GetListAsync(accountIds).ConfigureAwait(false);

            Assert.All(accounts, x => Assert.True(x.IsLocked));
        }

        [Fact]
        public async Task WhenUnlock_ThenSuccess()
        {
            var accountIds = (await Task.WhenAll(_create.Account.BuildAsync(), _create.Account.BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _accountsClient.UnlockAsync(accountIds).ConfigureAwait(false);

            var accounts = await _accountsClient.GetListAsync(accountIds).ConfigureAwait(false);

            Assert.All(accounts, x => Assert.False(x.IsLocked));
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var accountIds = (await Task.WhenAll(_create.Account.BuildAsync(), _create.Account.BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _accountsClient.DeleteAsync(accountIds).ConfigureAwait(false);

            var accounts = await _accountsClient.GetListAsync(accountIds).ConfigureAwait(false);

            Assert.All(accounts, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var accountIds = (await Task.WhenAll(_create.Account.BuildAsync(), _create.Account.BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _accountsClient.RestoreAsync(accountIds).ConfigureAwait(false);
            
            var accounts = await _accountsClient.GetListAsync(accountIds).ConfigureAwait(false);

            Assert.All(accounts, x => Assert.False(x.IsDeleted));
        }
    }
}
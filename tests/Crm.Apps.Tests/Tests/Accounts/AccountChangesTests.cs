using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Accounts.Clients;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Accounts.RequestParameters;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Accounts
{
    public class AccountChangesTests
    {
        private readonly ICreate _create;
        private readonly IAccountsClient _accountsClient;
        private readonly IAccountChangesClient _accountChangesClient;

        public AccountChangesTests(
            ICreate create,
            IAccountsClient accountsClient,
            IAccountChangesClient accountChangesClient)
        {
            _create = create;
            _accountsClient = accountsClient;
            _accountChangesClient = accountChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();

            var updateRequest = new AccountUpdateRequest
            {
                Id = account.Id,
                Type = AccountType.MlmSystem,
                IsLocked = true,
                IsDeleted = true,
                Settings = new List<AccountSetting>
                {
                    new AccountSetting
                    {
                        Type = AccountSettingType.PartnersEnabled
                    }
                }
            };

            await _accountsClient.UpdateAsync(updateRequest);
            var getPagedListRequest = new AccountChangeGetPagedListRequest
            {
                AccountId = account.Id,
                OrderBy = "asc",
                SortBy = "CreateDateTime"
            };

            var actualChanges = await _accountChangesClient.GetPagedListAsync(getPagedListRequest);

            Assert.NotEmpty(actualChanges);
            Assert.True(actualChanges.All(x => x.AccountId == account.Id));
            Assert.True(actualChanges.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(actualChanges.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(actualChanges.First().OldValueJson.IsEmpty());
            Assert.True(actualChanges.First().NewValueJson.IsNotEmpty());
            Assert.NotNull(actualChanges.First().NewValueJson.FromJsonString<Account>());
            Assert.True(actualChanges.Last().OldValueJson.IsNotEmpty());
            Assert.NotNull(actualChanges.Last().OldValueJson.FromJsonString<Account>());
            Assert.True(actualChanges.Last().NewValueJson.IsNotEmpty());
            Assert.NotNull(actualChanges.Last().NewValueJson.FromJsonString<Account>());
            Assert.False(actualChanges.Last().OldValueJson.FromJsonString<Account>().IsLocked);
            Assert.True(actualChanges.Last().NewValueJson.FromJsonString<Account>().IsLocked);
        }
    }
}
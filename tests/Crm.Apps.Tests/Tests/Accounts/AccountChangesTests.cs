using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Dsl.Creator;
using Crm.Clients.Accounts.Clients;
using Crm.Clients.Accounts.Models;
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

        public AccountChangesTests(ICreate create, IAccountsClient accountsClient,
            IAccountChangesClient accountChangesClient)
        {
            _create = create;
            _accountsClient = accountsClient;
            _accountChangesClient = accountChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            account.IsLocked = true;
            await _accountsClient.UpdateAsync(account).ConfigureAwait(false);

            var changes = await _accountChangesClient
                .GetPagedListAsync(accountId: account.Id, sortBy: "CreateDateTime", orderBy: "asc")
                .ConfigureAwait(false);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.AccountId == account.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<Account>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<Account>().IsLocked);
            Assert.True(changes.Last().NewValueJson.FromJsonString<Account>().IsLocked);
        }
    }
}
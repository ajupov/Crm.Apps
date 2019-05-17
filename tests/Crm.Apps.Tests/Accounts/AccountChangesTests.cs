using System.Linq;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Clients.AccountChanges;
using Crm.Clients.Accounts.Clients.Accounts;
using Crm.Clients.Accounts.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Accounts
{
    public class AccountChangesTests
    {
        private readonly IAccountsClient _accountsClient;
        private readonly IAccountChangesClient _accountChangesClient;

        public AccountChangesTests(IAccountsClient accountsClient, IAccountChangesClient accountChangesClient)
        {
            _accountsClient = accountsClient;
            _accountChangesClient = accountChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var accountId = await _accountsClient.CreateAsync().ConfigureAwait(false);
            var account = await _accountsClient.GetAsync(accountId).ConfigureAwait(false);

            account.IsLocked = true;

            await _accountsClient.UpdateAsync(account).ConfigureAwait(false);

            var changes = await _accountChangesClient
                .GetPagedListAsync(accountId: accountId, sortBy: nameof(Account.CreateDateTime), orderBy: "desc")
                .ConfigureAwait(false);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.AccountId == accountId));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));

            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<Account>());

            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.First().OldValueJson.FromJsonString<Account>().IsLocked);
            Assert.True(changes.First().NewValueJson.FromJsonString<Account>().IsLocked);
        }
    }
}
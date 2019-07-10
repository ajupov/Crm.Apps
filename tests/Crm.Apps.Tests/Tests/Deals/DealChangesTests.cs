using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Deals.Clients;
using Crm.Clients.Deals.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Deals
{
    public class DealChangesTests
    {
        private readonly ICreate _create;
        private readonly IDealsClient _dealsClient;
        private readonly IDealChangesClient _dealChangesClient;

        public DealChangesTests(ICreate create, IDealsClient dealsClient,
            IDealChangesClient dealChangesClient)
        {
            _create = create;
            _dealsClient = dealsClient;
            _dealChangesClient = dealChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var type = await _create.DealType.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var status = await _create.DealStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var deal = await _create.Deal.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                .BuildAsync().ConfigureAwait(false);
            deal.IsDeleted = true;
            await _dealsClient.UpdateAsync(deal).ConfigureAwait(false);

            var changes = await _dealChangesClient
                .GetPagedListAsync(dealId: deal.Id, sortBy: "CreateDateTime", orderBy: "asc")
                .ConfigureAwait(false);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.DealId == deal.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<Deal>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<Deal>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<Deal>().IsDeleted);
        }
    }
}
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
    public class DealStatusChangesTests
    {
        private readonly ICreate _create;
        private readonly IDealStatusesClient _dealStatusesClient;
        private readonly IDealStatusChangesClient _statusChangesClient;

        public DealStatusChangesTests(ICreate create, IDealStatusesClient dealStatusesClient,
            IDealStatusChangesClient statusChangesClient)
        {
            _create = create;
            _dealStatusesClient = dealStatusesClient;
            _statusChangesClient = statusChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var status = await _create.DealStatus.WithAccountId(account.Id).BuildAsync();
            status.Name = "Test2";
            status.IsDeleted = true;
            await _dealStatusesClient.UpdateAsync(status);

            var changes = await _statusChangesClient
                .GetPagedListAsync(statusId: status.Id, sortBy: "CreateDateTime", orderBy: "asc")
                ;

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.StatusId == status.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<DealStatus>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<DealStatus>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<DealStatus>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<DealStatus>().Name, status.Name);
        }
    }
}
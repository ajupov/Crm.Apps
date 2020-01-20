using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Json;
using Ajupov.Utils.All.String;
using Crm.Apps.Clients.Deals.Clients;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.RequestParameters;
using Crm.Apps.Tests.Creator;
using Xunit;

namespace Crm.Apps.Tests.Tests.Deals
{
    public class DealStatusChangesTests
    {
        private readonly ICreate _create;
        private readonly IDealStatusesClient _dealStatusesClient;
        private readonly IDealStatusChangesClient _statusChangesClient;

        public DealStatusChangesTests(
            ICreate create,
            IDealStatusesClient dealStatusesClient,
            IDealStatusChangesClient statusChangesClient)
        {
            _create = create;
            _dealStatusesClient = dealStatusesClient;
            _statusChangesClient = statusChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var status = await _create.DealStatus.BuildAsync();

            status.Name = "Test2";
            status.IsDeleted = true;

            await _dealStatusesClient.UpdateAsync(status);

            var request = new DealStatusChangeGetPagedListRequestParameter
            {
                StatusId = status.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var changes = await _statusChangesClient.GetPagedListAsync(request);

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
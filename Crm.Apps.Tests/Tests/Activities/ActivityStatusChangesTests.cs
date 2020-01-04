using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Activities
{
    public class ActivityStatusChangesTests
    {
        private readonly ICreate _create;
        private readonly IActivityStatusesClient _activityStatusesClient;
        private readonly IActivityStatusChangesClient _statusChangesClient;

        public ActivityStatusChangesTests(
            ICreate create,
            IActivityStatusesClient activityStatusesClient,
            IActivityStatusChangesClient statusChangesClient)
        {
            _create = create;
            _activityStatusesClient = activityStatusesClient;
            _statusChangesClient = statusChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var status = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();

            var updateRequest = new ActivityStatusUpdateRequest
            {
                Id = status.Id,
                Name = "Test2",
                IsDeleted = true
            };

            await _activityStatusesClient.UpdateAsync(updateRequest);

            var getPagedListRequest = new ActivityStatusChangeGetPagedListRequest
            {
                StatusId = status.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var changes = await _statusChangesClient.GetPagedListAsync(getPagedListRequest);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.StatusId == status.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<ActivityStatus>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<ActivityStatus>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<ActivityStatus>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<ActivityStatus>().Name, status.Name);
        }
    }
}
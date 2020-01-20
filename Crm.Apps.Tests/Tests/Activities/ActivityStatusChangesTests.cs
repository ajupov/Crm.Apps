using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Json;
using Ajupov.Utils.All.String;
using Crm.Apps.Clients.Activities.Clients;
using Crm.Apps.Clients.Activities.Models;
using Crm.Apps.Clients.Activities.RequestParameters;
using Crm.Apps.Tests.Creator;
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
            var status = await _create.ActivityStatus.BuildAsync();

            status.Name = "Test2";
            status.IsDeleted = true;

            await _activityStatusesClient.UpdateAsync(status);

            var getPagedListRequest = new ActivityStatusChangeGetPagedListRequestParameter
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
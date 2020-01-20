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
    public class ActivityChangesTests
    {
        private readonly ICreate _create;
        private readonly IActivitiesClient _activitiesClient;
        private readonly IActivityChangesClient _activityChangesClient;

        public ActivityChangesTests(
            ICreate create,
            IActivitiesClient activitiesClient,
            IActivityChangesClient activityChangesClient)
        {
            _create = create;
            _activitiesClient = activitiesClient;
            _activityChangesClient = activityChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var type = await _create.ActivityType.BuildAsync();
            var status = await _create.ActivityStatus.BuildAsync();
            var activity = await _create.Activity
                .WithTypeId(type.Id)
                .WithStatusId(status.Id)
                .BuildAsync();

            activity.Name = "Test1";

            await _activitiesClient.UpdateAsync(activity);

            var request = new ActivityChangeGetPagedListRequestParameter
            {
                ActivityId = activity.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var changes = await _activityChangesClient.GetPagedListAsync(request);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.ActivityId == activity.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<Activity>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<Activity>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<Activity>().IsDeleted);
        }
    }
}
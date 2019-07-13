using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Activities
{
    public class ActivityChangesTests
    {
        private readonly ICreate _create;
        private readonly IActivitiesClient _activitiesClient;
        private readonly IActivityChangesClient _activityChangesClient;

        public ActivityChangesTests(ICreate create, IActivitiesClient activitiesClient,
            IActivityChangesClient activityChangesClient)
        {
            _create = create;
            _activitiesClient = activitiesClient;
            _activityChangesClient = activityChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();
            var status = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync();
            var activity = await _create.Activity.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                .BuildAsync();
            activity.IsDeleted = true;
            await _activitiesClient.UpdateAsync(activity);

            var changes = await _activityChangesClient
                .GetPagedListAsync(activityId: activity.Id, sortBy: "CreateDateTime", orderBy: "asc")
                ;

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
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
    public class ActivityStatusChangesTests
    {
        private readonly ICreate _create;
        private readonly IActivityStatusesClient _activityStatusesClient;
        private readonly IActivityStatusChangesClient _statusChangesClient;

        public ActivityStatusChangesTests(ICreate create, IActivityStatusesClient activityStatusesClient,
            IActivityStatusChangesClient statusChangesClient)
        {
            _create = create;
            _activityStatusesClient = activityStatusesClient;
            _statusChangesClient = statusChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var status = await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            status.Name = "Test2";
            status.IsDeleted = true;
            await _activityStatusesClient.UpdateAsync(status).ConfigureAwait(false);

            var changes = await _statusChangesClient
                .GetPagedListAsync(statusId: status.Id, sortBy: "CreateDateTime", orderBy: "asc")
                .ConfigureAwait(false);

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
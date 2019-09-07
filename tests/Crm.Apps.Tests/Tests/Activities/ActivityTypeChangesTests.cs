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
    public class ActivityTypeChangesTests
    {
        private readonly ICreate _create;
        private readonly IActivityTypesClient _activityTypesClient;
        private readonly IActivityTypeChangesClient _typeChangesClient;

        public ActivityTypeChangesTests(
            ICreate create,
            IActivityTypesClient activityTypesClient,
            IActivityTypeChangesClient typeChangesClient)
        {
            _create = create;
            _activityTypesClient = activityTypesClient;
            _typeChangesClient = typeChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var type = await _create.ActivityType.WithAccountId(account.Id).BuildAsync();

            var updateRequest = new ActivityTypeUpdateRequest
            {
                Name = "Test2",
                IsDeleted = true
            };

            await _activityTypesClient.UpdateAsync(updateRequest);

            var getPagedListRequest = new ActivityTypeChangeGetPagedListRequest
            {
                TypeId = type.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var changes = await _typeChangesClient.GetPagedListAsync(getPagedListRequest);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.TypeId == type.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<ActivityType>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<ActivityType>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<ActivityType>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<ActivityType>().Name, type.Name);
        }
    }
}
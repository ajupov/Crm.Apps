using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;
using Crm.Common.Types;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Activities
{
    public class ActivityAttributeChangesTests
    {
        private readonly ICreate _create;
        private readonly IActivityAttributesClient _activityAttributesClient;
        private readonly IActivityAttributeChangesClient _attributeChangesClient;

        public ActivityAttributeChangesTests(
            ICreate create,
            IActivityAttributesClient activityAttributesClient,
            IActivityAttributeChangesClient attributeChangesClient)
        {
            _create = create;
            _activityAttributesClient = activityAttributesClient;
            _attributeChangesClient = attributeChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attribute = await _create.ActivityAttribute.WithAccountId(account.Id).BuildAsync();

            var updateRequest = new ActivityAttributeUpdateRequest
            {
                Type = AttributeType.Link,
                Key = "TestLink",
                IsDeleted = true
            };

            await _activityAttributesClient.UpdateAsync(updateRequest);

            var getPagesListRequest = new ActivityAttributeChangeGetPagedListRequest
            {
                AttributeId = attribute.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var changes = await _attributeChangesClient.GetPagedListAsync(getPagesListRequest);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.AttributeId == attribute.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<ActivityAttribute>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<ActivityAttribute>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<ActivityAttribute>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<ActivityAttribute>().Key, attribute.Key);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<ActivityAttribute>().Type, attribute.Type);
        }
    }
}
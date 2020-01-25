using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Json;
using Ajupov.Utils.All.String;
using Crm.Apps.Tests.Extensions;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.v1.Clients.Activities.Clients;
using Crm.Apps.v1.Clients.Activities.Models;
using Crm.Apps.v1.Clients.Activities.RequestParameters;
using Crm.Common.All.Types.AttributeType;
using Xunit;

namespace Crm.Apps.Tests.Tests.Activities
{
    public class ActivityAttributeChangesTests
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly ICreate _create;
        private readonly IActivityAttributesClient _activityAttributesClient;
        private readonly IActivityAttributeChangesClient _attributeChangesClient;

        public ActivityAttributeChangesTests(
            IAccessTokenGetter accessTokenGetter,
            ICreate create,
            IActivityAttributesClient activityAttributesClient,
            IActivityAttributeChangesClient attributeChangesClient)
        {
            _accessTokenGetter = accessTokenGetter;
            _create = create;
            _activityAttributesClient = activityAttributesClient;
            _attributeChangesClient = attributeChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var attribute = await _create.ActivityAttribute.BuildAsync();

            attribute.Type = AttributeType.Link;
            attribute.Key = "Test".WithGuid();
            attribute.IsDeleted = true;

            await _activityAttributesClient.UpdateAsync(accessToken, attribute);

            var request = new ActivityAttributeChangeGetPagedListRequestParameter
            {
                AttributeId = attribute.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var changes = await _attributeChangesClient.GetPagedListAsync(accessToken, request);

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
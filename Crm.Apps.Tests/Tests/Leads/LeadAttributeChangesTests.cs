using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Json;
using Ajupov.Utils.All.String;
using Crm.Apps.Tests.Extensions;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.v1.Clients.Leads.Clients;
using Crm.Apps.v1.Clients.Leads.Models;
using Crm.Apps.v1.Clients.Leads.RequestParameters;
using Crm.Common.All.Types.AttributeType;
using Xunit;

namespace Crm.Apps.Tests.Tests.Leads
{
    public class LeadAttributeChangesTests
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly ICreate _create;
        private readonly ILeadAttributesClient _leadAttributesClient;
        private readonly ILeadAttributeChangesClient _attributeChangesClient;

        public LeadAttributeChangesTests(
            IAccessTokenGetter accessTokenGetter,
            ICreate create,
            ILeadAttributesClient leadAttributesClient,
            ILeadAttributeChangesClient attributeChangesClient)
        {
            _accessTokenGetter = accessTokenGetter;
            _create = create;
            _leadAttributesClient = leadAttributesClient;
            _attributeChangesClient = attributeChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var attribute = await _create.LeadAttribute.BuildAsync();

            attribute.Type = AttributeType.Link;
            attribute.Key = "TestLink".WithGuid();
            attribute.IsDeleted = true;

            await _leadAttributesClient.UpdateAsync(accessToken, attribute);

            var request = new LeadAttributeChangeGetPagedListRequestParameter
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
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<LeadAttribute>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<LeadAttribute>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<LeadAttribute>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<LeadAttribute>().Key, attribute.Key);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<LeadAttribute>().Type, attribute.Type);
        }
    }
}
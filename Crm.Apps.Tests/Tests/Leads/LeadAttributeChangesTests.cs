using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Json;
using Crm.Apps.Clients.Leads.Clients;
using Crm.Apps.Clients.Leads.Models;
using Crm.Apps.Tests.Creator;
using Crm.Common.All.Types.AttributeType;
using Xunit;

namespace Crm.Apps.Tests.Tests.Leads
{
    public class LeadAttributeChangesTests
    {
        private readonly ICreate _create;
        private readonly ILeadAttributesClient _leadAttributesClient;
        private readonly ILeadAttributeChangesClient _attributeChangesClient;

        public LeadAttributeChangesTests(ICreate create, ILeadAttributesClient leadAttributesClient,
            ILeadAttributeChangesClient attributeChangesClient)
        {
            _create = create;
            _leadAttributesClient = leadAttributesClient;
            _attributeChangesClient = attributeChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            
            var attribute = await _create.LeadAttribute.BuildAsync();
            attribute.Type = AttributeType.Link;
            attribute.Key = "TestLink";
            attribute.IsDeleted = true;
            await _leadAttributesClient.UpdateAsync(attribute);

            var changes = await _attributeChangesClient
                .GetPagedListAsync(attributeId: attribute.Id, sortBy: "CreateDateTime", orderBy: "asc")
                ;

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
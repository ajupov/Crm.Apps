using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Dsl.Creator;
using Crm.Clients.Leads.Clients;
using Crm.Clients.Leads.Models;
using Crm.Common.Types;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
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
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attribute = await _create.LeadAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            attribute.Type = AttributeType.Link;
            attribute.Key = "TestLink";
            attribute.IsDeleted = true;
            await _leadAttributesClient.UpdateAsync(attribute).ConfigureAwait(false);

            var changes = await _attributeChangesClient
                .GetPagedListAsync(attributeId: attribute.Id, sortBy: "CreateDateTime", orderBy: "asc")
                .ConfigureAwait(false);

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
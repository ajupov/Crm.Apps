using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Deals.Clients;
using Crm.Clients.Deals.Models;
using Crm.Common.Types;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Deals
{
    public class DealAttributeChangesTests
    {
        private readonly ICreate _create;
        private readonly IDealAttributesClient _dealAttributesClient;
        private readonly IDealAttributeChangesClient _attributeChangesClient;

        public DealAttributeChangesTests(ICreate create, IDealAttributesClient dealAttributesClient,
            IDealAttributeChangesClient attributeChangesClient)
        {
            _create = create;
            _dealAttributesClient = dealAttributesClient;
            _attributeChangesClient = attributeChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var attribute = await _create.DealAttribute.WithAccountId(account.Id).BuildAsync();
            attribute.Type = AttributeType.Link;
            attribute.Key = "TestLink";
            attribute.IsDeleted = true;
            await _dealAttributesClient.UpdateAsync(attribute);

            var changes = await _attributeChangesClient
                .GetPagedListAsync(attributeId: attribute.Id, sortBy: "CreateDateTime", orderBy: "asc")
                ;

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.AttributeId == attribute.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<DealAttribute>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<DealAttribute>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<DealAttribute>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<DealAttribute>().Key, attribute.Key);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<DealAttribute>().Type, attribute.Type);
        }
    }
}
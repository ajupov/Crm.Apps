using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Json;
using Ajupov.Utils.All.String;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.v1.Clients.Contacts.Clients;
using Crm.Apps.v1.Clients.Contacts.Models;
using Crm.Apps.v1.Clients.Contacts.RequestParameters;
using Crm.Common.All.Types.AttributeType;
using Xunit;

namespace Crm.Apps.Tests.Tests.Contacts
{
    public class ContactAttributeChangesTests
    {
        private readonly ICreate _create;
        private readonly IContactAttributesClient _contactAttributesClient;
        private readonly IContactAttributeChangesClient _attributeChangesClient;

        public ContactAttributeChangesTests(
            ICreate create,
            IContactAttributesClient contactAttributesClient,
            IContactAttributeChangesClient attributeChangesClient)
        {
            _create = create;
            _contactAttributesClient = contactAttributesClient;
            _attributeChangesClient = attributeChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var attribute = await _create.ContactAttribute.BuildAsync();

            attribute.Type = AttributeType.Link;
            attribute.Key = "TestLink";
            attribute.IsDeleted = true;

            await _contactAttributesClient.UpdateAsync(attribute);

            var request = new ContactAttributeChangeGetPagedListRequestParameter
            {
                AttributeId = attribute.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var changes = await _attributeChangesClient.GetPagedListAsync(request);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.AttributeId == attribute.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<ContactAttribute>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<ContactAttribute>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<ContactAttribute>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<ContactAttribute>().Key, attribute.Key);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<ContactAttribute>().Type, attribute.Type);
        }
    }
}
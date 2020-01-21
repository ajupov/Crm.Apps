using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Json;
using Ajupov.Utils.All.String;
using Crm.Apps.Tests.Creator;
using Crm.Apps.v1.Clients.Contacts.Clients;
using Crm.Apps.v1.Clients.Contacts.Models;
using Crm.Apps.v1.Clients.Contacts.RequestParameters;
using Xunit;

namespace Crm.Apps.Tests.Tests.Contacts
{
    public class ContactChangesTests
    {
        private readonly ICreate _create;
        private readonly IContactsClient _contactsClient;
        private readonly IContactChangesClient _contactChangesClient;

        public ContactChangesTests(
            ICreate create,
            IContactsClient contactsClient,
            IContactChangesClient contactChangesClient)
        {
            _create = create;
            _contactsClient = contactsClient;
            _contactChangesClient = contactChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var leadSource = await _create.LeadSource.BuildAsync();
            var lead = await _create.Lead
                .WithSourceId(leadSource.Id)
                .BuildAsync();
            var contact = await _create.Contact.WithLeadId(lead.Id).BuildAsync();

            contact.IsDeleted = true;

            await _contactsClient.UpdateAsync(contact);

            var request = new ContactChangeGetPagedListRequestParameter
            {
                ContactId = contact.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var changes = await _contactChangesClient.GetPagedListAsync(request);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.ContactId == contact.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<Contact>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<Contact>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<Contact>().IsDeleted);
        }
    }
}
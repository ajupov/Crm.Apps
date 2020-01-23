using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.v1.Clients.Contacts.Clients;
using Crm.Apps.v1.Clients.Contacts.Models;
using Crm.Apps.v1.Clients.Contacts.RequestParameters;
using Xunit;

namespace Crm.Apps.Tests.Tests.Contacts
{
    public class ContactCommentsTests
    {
        private readonly ICreate _create;
        private readonly IContactCommentsClient _contactCommentsClient;

        public ContactCommentsTests(ICreate create, IContactCommentsClient contactCommentsClient)
        {
            _create = create;
            _contactCommentsClient = contactCommentsClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var leadSource = await _create.LeadSource.BuildAsync();
            var lead = await _create.Lead
                .WithSourceId(leadSource.Id)
                .BuildAsync();
            var contact = await _create.Contact
                .WithLeadId(lead.Id)
                .BuildAsync();
            await Task.WhenAll(
                _create.ContactComment
                    .WithContactId(contact.Id)
                    .BuildAsync(),
                _create.ContactComment
                    .WithContactId(contact.Id)
                    .BuildAsync());

            var request = new ContactCommentGetPagedListRequestParameter
            {
                ContactId = contact.Id,
                SortBy = "CreateDateTime",
                OrderBy = "desc"
            };

            var comments = await _contactCommentsClient.GetPagedListAsync(request);

            var results = comments
                .Skip(1)
                .Zip(comments, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(comments);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var leadSource = await _create.LeadSource.BuildAsync();
            var lead = await _create.Lead
                .WithSourceId(leadSource.Id)
                .BuildAsync();
            var contact = await _create.Contact
                .WithLeadId(lead.Id)
                .BuildAsync();

            var comment = new ContactComment
            {
                ContactId = contact.Id,
                Value = "Test"
            };

            await _contactCommentsClient.CreateAsync(comment);

            var request = new ContactCommentGetPagedListRequestParameter
            {
                ContactId = contact.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var createdComment = (await _contactCommentsClient.GetPagedListAsync(request)).First();

            Assert.NotNull(createdComment);
            Assert.Equal(comment.ContactId, createdComment.ContactId);
            Assert.True(!createdComment.CommentatorUserId.IsEmpty());
            Assert.Equal(comment.Value, createdComment.Value);
            Assert.True(createdComment.CreateDateTime.IsMoreThanMinValue());
        }
    }
}
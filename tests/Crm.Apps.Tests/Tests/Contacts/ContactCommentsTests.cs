using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Contacts.Clients;
using Crm.Clients.Contacts.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
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
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var contact = await _create.Contact.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            await Task.WhenAll(
                    _create.ContactComment.WithContactId(contact.Id).BuildAsync(),
                    _create.ContactComment.WithContactId(contact.Id).BuildAsync())
                .ConfigureAwait(false);

            var comments = await _contactCommentsClient
                .GetPagedListAsync(contact.Id, sortBy: "CreateDateTime", orderBy: "desc").ConfigureAwait(false);

            var results = comments.Skip(1)
                .Zip(comments, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(comments);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var contact = await _create.Contact.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);

            var comment = new ContactComment
            {
                ContactId = contact.Id,
                Value = "Test"
            };

            await _contactCommentsClient.CreateAsync(comment).ConfigureAwait(false);

            var createdComment = (await _contactCommentsClient.GetPagedListAsync(contact.Id, sortBy: "CreateDateTime",
                orderBy: "asc").ConfigureAwait(false)).First();

            Assert.NotNull(createdComment);
            Assert.Equal(comment.ContactId, createdComment.ContactId);
            Assert.True(!createdComment.CommentatorUserId.IsEmpty());
            Assert.Equal(comment.Value, createdComment.Value);
            Assert.True(createdComment.CreateDateTime.IsMoreThanMinValue());
        }
    }
}
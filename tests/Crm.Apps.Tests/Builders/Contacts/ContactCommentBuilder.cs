using System;
using System.Threading.Tasks;
using Crm.Clients.Contacts.Clients;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Contacts
{
    public class ContactCommentBuilder : IContactCommentBuilder
    {
        private readonly Clients.Contacts.Models.ContactComment _contactComment;
        private readonly IContactCommentsClient _contactCommentsClient;

        public ContactCommentBuilder(IContactCommentsClient contactCommentsClient)
        {
            _contactCommentsClient = contactCommentsClient;
            _contactComment = new Clients.Contacts.Models.ContactComment
            {
                ContactId = Guid.Empty,
                CommentatorUserId = Guid.Empty,
                Value = "Test"
            };
        }

        public ContactCommentBuilder WithContactId(Guid contactId)
        {
            _contactComment.ContactId = contactId;

            return this;
        }

        public Task BuildAsync()
        {
            if (_contactComment.ContactId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_contactComment.ContactId));
            }

            return _contactCommentsClient.CreateAsync(_contactComment);
        }
    }
}
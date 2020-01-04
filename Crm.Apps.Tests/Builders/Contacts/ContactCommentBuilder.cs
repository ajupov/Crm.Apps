using System;
using System.Threading.Tasks;
using Crm.Clients.Contacts.Clients;
using Crm.Clients.Contacts.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Contacts
{
    public class ContactCommentBuilder : IContactCommentBuilder
    {
        private readonly IContactCommentsClient _contactCommentsClient;
        private readonly ContactComment _contactComment;

        public ContactCommentBuilder(IContactCommentsClient contactCommentsClient)
        {
            _contactCommentsClient = contactCommentsClient;
            _contactComment = new ContactComment
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
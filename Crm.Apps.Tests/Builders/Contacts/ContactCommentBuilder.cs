using System;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.v1.Clients.Contacts.Clients;
using Crm.Apps.v1.Clients.Contacts.Models;

namespace Crm.Apps.Tests.Builders.Contacts
{
    public class ContactCommentBuilder : IContactCommentBuilder
    {
        private readonly IContactCommentsClient _contactCommentsClient;
        private readonly ContactComment _comment;

        public ContactCommentBuilder(IContactCommentsClient contactCommentsClient)
        {
            _contactCommentsClient = contactCommentsClient;
            _comment = new ContactComment
            {
                ContactId = Guid.Empty,
                Value = "Test"
            };
        }

        public ContactCommentBuilder WithContactId(Guid contactId)
        {
            _comment.ContactId = contactId;

            return this;
        }

        public Task BuildAsync()
        {
            if (_comment.ContactId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_comment.ContactId));
            }

            return _contactCommentsClient.CreateAsync(_comment);
        }
    }
}
using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Builders.Contacts
{
    public interface IContactCommentBuilder
    {
        ContactCommentBuilder WithContactId(Guid contactId);

        Task BuildAsync();
    }
}
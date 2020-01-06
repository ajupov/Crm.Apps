using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Contacts.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Contacts
{
    public interface IContactAttributeBuilder
    {
        ContactAttributeBuilder WithAccountId(Guid accountId);

        ContactAttributeBuilder WithType(AttributeType type);

        ContactAttributeBuilder WithKey(string key);

        ContactAttributeBuilder AsDeleted();

        Task<ContactAttribute> BuildAsync();
    }
}
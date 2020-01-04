using System;
using System.Threading.Tasks;
using Crm.Clients.Contacts.Models;
using Crm.Common.Types;

namespace Crm.Apps.Tests.Builders.Contacts
{
    public interface IContactAttributeBuilder
    {
        ContactAttributeBuilder WithAccountId(Guid accountId);

        ContactAttributeBuilder WithType(AttributeType type);

        ContactAttributeBuilder WithKey(string key);

        Task<ContactAttribute> BuildAsync();
    }
}
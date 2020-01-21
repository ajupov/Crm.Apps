using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Contacts.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Contacts
{
    public interface IContactAttributeBuilder
    {
        ContactAttributeBuilder WithType(AttributeType type);

        ContactAttributeBuilder WithKey(string key);

        ContactAttributeBuilder AsDeleted();

        Task<ContactAttribute> BuildAsync();
    }
}
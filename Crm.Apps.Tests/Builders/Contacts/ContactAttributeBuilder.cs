using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Contacts.Clients;
using Crm.Apps.Clients.Contacts.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Contacts
{
    public class ContactAttributeBuilder : IContactAttributeBuilder
    {
        private readonly IContactAttributesClient _contactAttributesClient;
        private readonly ContactAttribute _attribute;

        public ContactAttributeBuilder(IContactAttributesClient contactAttributesClient)
        {
            _contactAttributesClient = contactAttributesClient;
            _attribute = new ContactAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };
        }

        public ContactAttributeBuilder WithAccountId(Guid accountId)
        {
            _attribute.AccountId = accountId;

            return this;
        }

        public ContactAttributeBuilder WithType(AttributeType type)
        {
            _attribute.Type = type;

            return this;
        }

        public ContactAttributeBuilder WithKey(string key)
        {
            _attribute.Key = key;

            return this;
        }

        public ContactAttributeBuilder AsDeleted()
        {
            _attribute.IsDeleted = true;

            return this;
        }

        public async Task<ContactAttribute> BuildAsync()
        {
            var id = await _contactAttributesClient.CreateAsync(_attribute);

            return await _contactAttributesClient.GetAsync(id);
        }
    }
}
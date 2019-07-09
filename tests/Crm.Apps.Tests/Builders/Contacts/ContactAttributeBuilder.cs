using System;
using System.Threading.Tasks;
using Crm.Clients.Contacts.Clients;
using Crm.Clients.Contacts.Models;
using Crm.Common.Types;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Contacts
{
    public class ContactAttributeBuilder : IContactAttributeBuilder
    {
        private readonly IContactAttributesClient _contactAttributesClient;
        private readonly ContactAttribute _contactAttribute;

        public ContactAttributeBuilder(IContactAttributesClient contactAttributesClient)
        {
            _contactAttributesClient = contactAttributesClient;
            _contactAttribute = new ContactAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test"
            };
        }

        public ContactAttributeBuilder WithAccountId(Guid accountId)
        {
            _contactAttribute.AccountId = accountId;

            return this;
        }

        public ContactAttributeBuilder WithType(AttributeType type)
        {
            _contactAttribute.Type = type;

            return this;
        }

        public ContactAttributeBuilder WithKey(string key)
        {
            _contactAttribute.Key = key;

            return this;
        }

        public async Task<ContactAttribute> BuildAsync()
        {
            if (_contactAttribute.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_contactAttribute.AccountId));
            }

            var createdId = await _contactAttributesClient.CreateAsync(_contactAttribute).ConfigureAwait(false);

            return await _contactAttributesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}
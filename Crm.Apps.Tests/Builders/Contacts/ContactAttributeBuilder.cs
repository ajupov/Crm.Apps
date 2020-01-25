using System;
using System.Threading.Tasks;
using Crm.Apps.Tests.Extensions;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.v1.Clients.Contacts.Clients;
using Crm.Apps.v1.Clients.Contacts.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Contacts
{
    public class ContactAttributeBuilder : IContactAttributeBuilder
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly IContactAttributesClient _contactAttributesClient;
        private readonly ContactAttribute _attribute;

        public ContactAttributeBuilder(
            IAccessTokenGetter accessTokenGetter,
            IContactAttributesClient contactAttributesClient)
        {
            _contactAttributesClient = contactAttributesClient;
            _accessTokenGetter = accessTokenGetter;
            _attribute = new ContactAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test".WithGuid(),
                IsDeleted = false
            };
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
            var accessToken = await _accessTokenGetter.GetAsync();

            var id = await _contactAttributesClient.CreateAsync(accessToken, _attribute);

            return await _contactAttributesClient.GetAsync(accessToken, id);
        }
    }
}
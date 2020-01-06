using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Users.Clients;
using Crm.Apps.Clients.Users.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Users
{
    public class UserAttributeBuilder : IUserAttributeBuilder
    {
        private readonly IUserAttributesClient _userAttributesClient;
        private readonly UserAttribute _attribute;

        public UserAttributeBuilder(IUserAttributesClient userAttributesClient)
        {
            _userAttributesClient = userAttributesClient;
            _attribute = new UserAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };
        }

        public UserAttributeBuilder WithType(AttributeType type)
        {
            _attribute.Type = type;

            return this;
        }

        public UserAttributeBuilder WithKey(string key)
        {
            _attribute.Key = key;

            return this;
        }

        public UserAttributeBuilder AsDeleted()
        {
            _attribute.IsDeleted = true;

            return this;
        }

        public async Task<UserAttribute> BuildAsync()
        {
            var id = await _userAttributesClient.CreateAsync(_attribute);

            return await _userAttributesClient.GetAsync(id);
        }
    }
}
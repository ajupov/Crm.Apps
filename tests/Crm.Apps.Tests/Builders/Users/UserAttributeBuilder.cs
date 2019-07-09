using System;
using System.Threading.Tasks;
using Crm.Clients.Users.Clients;
using Crm.Clients.Users.Models;
using Crm.Common.Types;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Users
{
    public class UserAttributeBuilder : IUserAttributeBuilder
    {
        private readonly IUserAttributesClient _userAttributesClient;
        private readonly UserAttribute _userAttribute;

        public UserAttributeBuilder(IUserAttributesClient userAttributesClient)
        {
            _userAttributesClient = userAttributesClient;
            _userAttribute = new UserAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test"
            };
        }

        public UserAttributeBuilder WithAccountId(Guid accountId)
        {
            _userAttribute.AccountId = accountId;

            return this;
        }

        public UserAttributeBuilder WithType(AttributeType type)
        {
            _userAttribute.Type = type;

            return this;
        }

        public UserAttributeBuilder WithKey(string key)
        {
            _userAttribute.Key = key;

            return this;
        }

        public async Task<UserAttribute> BuildAsync()
        {
            if (_userAttribute.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_userAttribute.AccountId));
            }

            var createdId = await _userAttributesClient.CreateAsync(_userAttribute).ConfigureAwait(false);

            return await _userAttributesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}
using System;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Users.Clients.UserAttributes;
using Crm.Common.Types;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Dsl.Builders.UserAttribute
{
    public class UserAttributeBuilder : IUserAttributeBuilder
    {
        private readonly Clients.Users.Models.UserAttribute _userAttribute;
        private readonly IUserAttributesClient _userAttributesClient;

        public UserAttributeBuilder(IUserAttributesClient userAttributesClient)
        {
            _userAttributesClient = userAttributesClient;
            _userAttribute = new Clients.Users.Models.UserAttribute
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

        public async Task<Clients.Users.Models.UserAttribute> BuildAsync()
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
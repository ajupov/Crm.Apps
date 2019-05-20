using System;
using Crm.Clients.Users.Models;
using Crm.Common.Types;

namespace Crm.Apps.Tests.Dsl.Builders
{
    public class UserAttributeBuilder
    {
        private readonly UserAttribute _userAttribute;

        public UserAttributeBuilder(Guid accountId)
        {
            _userAttribute = new UserAttribute
            {
                AccountId = accountId,
                Type = AttributeType.Text,
                Key = "test"
            };
        }

        public UserAttributeBuilder WithKey(string key)
        {
            _userAttribute.Key = key;

            return this;
        }

        public UserAttribute Build()
        {
            return _userAttribute;
        }
    }
}
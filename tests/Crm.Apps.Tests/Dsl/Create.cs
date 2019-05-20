using System;
using Crm.Apps.Tests.Dsl.Builders;

namespace Crm.Apps.Tests.Dsl
{
    public static class Create
    {
        public static AccountBuilder Account()
        {
            return new AccountBuilder();
        }

        public static UserBuilder User(Guid accountId)
        {
            return new UserBuilder(accountId);
        }

        public static UserAttributeBuilder UserAttribute(Guid accountId)
        {
            return new UserAttributeBuilder(accountId);
        }

        public static UserGroupBuilder UserGroup(Guid accountId)
        {
            return new UserGroupBuilder(accountId);
        }
    }
}
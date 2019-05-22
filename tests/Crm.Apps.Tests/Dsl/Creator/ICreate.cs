using Crm.Apps.Tests.Dsl.Builders.Account;
using Crm.Apps.Tests.Dsl.Builders.User;
using Crm.Apps.Tests.Dsl.Builders.UserAttribute;
using Crm.Apps.Tests.Dsl.Builders.UserGroup;

namespace Crm.Apps.Tests.Dsl.Creator
{
    public interface ICreate
    {
        IAccountBuilder Account { get; }

        IUserBuilder User { get; }

        IUserAttributeBuilder UserAttribute { get; }

        IUserGroupBuilder UserGroup { get; }
    }
}
using System;
using System.Threading.Tasks;
using Crm.Common.UserContext;

namespace Crm.Apps.Tests.Dsl.Builders.UserGroup
{
    public interface IUserGroupBuilder
    {
        UserGroupBuilder WithAccountId(Guid accountId);

        UserGroupBuilder WithName(string name);

        UserGroupBuilder WithPermission(Permission permission);

        Task<Clients.Users.Models.UserGroup> BuildAsync();
    }
}
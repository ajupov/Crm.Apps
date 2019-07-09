using System;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Common.UserContext;

namespace Crm.Apps.Tests.Builders.Users
{
    public interface IUserGroupBuilder
    {
        UserGroupBuilder WithAccountId(Guid accountId);

        UserGroupBuilder WithName(string name);

        UserGroupBuilder WithPermission(Permission permission);

        Task<UserGroup> BuildAsync();
    }
}
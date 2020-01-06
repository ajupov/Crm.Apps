using System.Threading.Tasks;
using Crm.Apps.Clients.Users.Models;
using Crm.Common.All.UserContext;

namespace Crm.Apps.Tests.Builders.Users
{
    public interface IUserGroupBuilder
    {
        UserGroupBuilder WithName(string name);

        UserGroupBuilder WithRole(Role role);

        UserGroupBuilder AsDeleted();

        Task<UserGroup> BuildAsync();
    }
}
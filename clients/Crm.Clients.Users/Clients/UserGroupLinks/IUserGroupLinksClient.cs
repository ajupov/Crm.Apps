using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Clients.Users.Clients.UserGroupLinks
{
    public interface IUserGroupLinksClient
    {
        Task AddToGroupAsync(Guid userId, Guid groupId, CancellationToken ct = default);

        Task DeleteFromGroupAsync(Guid userId, Guid groupId, CancellationToken ct = default);
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Clients.Users.Clients.UserPostLinks
{
    public interface IUserPostLinksClient
    {
        Task AddAsync(Guid userId, Guid postId, CancellationToken ct = default);

        Task DeleteAsync(Guid userId, Guid postId, CancellationToken ct = default);
    }
}
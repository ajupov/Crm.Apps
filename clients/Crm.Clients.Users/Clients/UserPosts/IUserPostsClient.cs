using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;

namespace Crm.Clients.Users.Clients.UserPosts
{
    public interface IUserPostsClient
    {
        Task<UserPost> GetAsync(Guid id, CancellationToken ct = default);

        Task<ICollection<UserPost>> GetListAsync(ICollection<Guid> ids, CancellationToken ct = default);

        Task<ICollection<UserPost>> GetPagedListAsync(Guid? accountId = default, string name = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(UserPost post, CancellationToken ct = default);

        Task UpdateAsync(UserPost post, CancellationToken ct = default);

        Task DeleteAsync(ICollection<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(ICollection<Guid> ids, CancellationToken ct = default);
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;

namespace Crm.Clients.Users.Clients
{
    public interface IUserGroupsClient
    {
        Task<UserGroup> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<UserGroup>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<UserGroup>> GetPagedListAsync(Guid? accountId = default, string name = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(UserGroup group, CancellationToken ct = default);

        Task UpdateAsync(UserGroup group, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;

namespace Crm.Apps.Areas.Users.Services
{
    public interface IUserGroupsService
    {
        Task<UserGroup> GetAsync(Guid id, CancellationToken ct);

        Task<List<UserGroup>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<UserGroup>> GetPagedListAsync(Guid? accountId, string name, bool? isDeleted,
            DateTime? minCreateDate, DateTime? maxCreateDate, int offset, int limit, string sortBy, string orderBy,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, UserGroup group, CancellationToken ct);

        Task UpdateAsync(Guid userId, UserGroup oldGroup, UserGroup newGroup, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.Parameters;

namespace Crm.Apps.Users.Services
{
    public interface IUserGroupsService
    {
        Task<UserGroup> GetAsync(Guid id, CancellationToken ct);

        Task<List<UserGroup>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<UserGroup>> GetPagedListAsync(UserGroupGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, UserGroup group, CancellationToken ct);

        Task UpdateAsync(Guid userId, UserGroup oldGroup, UserGroup newGroup, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
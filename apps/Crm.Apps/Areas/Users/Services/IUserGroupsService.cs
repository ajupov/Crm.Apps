using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.RequestParameters;

namespace Crm.Apps.Areas.Users.Services
{
    public interface IUserGroupsService
    {
        Task<UserGroup> GetAsync(Guid id, CancellationToken ct);

        Task<List<UserGroup>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<UserGroup>> GetPagedListAsync(UserGroupGetPagedListRequestParameter request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, UserGroup group, CancellationToken ct);

        Task UpdateAsync(Guid userId, UserGroup oldGroup, UserGroup newGroup, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
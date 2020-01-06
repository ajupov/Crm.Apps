using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Users.Models;
using Crm.Apps.Clients.Users.RequestParameters;

namespace Crm.Apps.Clients.Users.Clients
{
    public interface IUserGroupsClient
    {
        Task<UserGroup> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<UserGroup>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<UserGroup>> GetPagedListAsync(
            UserGroupGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(UserGroup group, CancellationToken ct = default);

        Task UpdateAsync(UserGroup group, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
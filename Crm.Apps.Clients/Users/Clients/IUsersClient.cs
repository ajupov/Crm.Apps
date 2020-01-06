using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Users.Models;
using Crm.Apps.Clients.Users.RequestParameters;

namespace Crm.Apps.Clients.Users.Clients
{
    public interface IUsersClient
    {
        Task<Dictionary<string, UserGender>> GetGendersAsync(CancellationToken ct = default);

        Task<User> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<User>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<User>> GetPagedListAsync(UserGetPagedListRequestParameter request, CancellationToken ct = default);

        Task<Guid> CreateAsync(User user, CancellationToken ct = default);

        Task UpdateAsync(User user, CancellationToken ct = default);

        Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
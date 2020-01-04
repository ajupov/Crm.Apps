using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.RequestParameters;

namespace Crm.Apps.Users.Services
{
    public interface IUsersService
    {
        Task<User> GetAsync(Guid id, CancellationToken ct);

        Task<List<User>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<User>> GetPagedListAsync(UserGetPagedListRequestParameter request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, User user, CancellationToken ct);

        Task UpdateAsync(Guid userId, User oldUser, User newUser, CancellationToken ct);

        Task LockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task UnlockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
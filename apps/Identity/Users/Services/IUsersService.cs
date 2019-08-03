using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Identity.Users.Models;
using Identity.Users.Parameters;

namespace Identity.Users.Services
{
    public interface IUsersService
    {
        Task<User> GetAsync(Guid id, CancellationToken ct);

        Task<User[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<User[]> GetPagedListAsync(UserGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(User user, CancellationToken ct);

        Task UpdateAsync(User oldUser, User user, CancellationToken ct);

        Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct);
    }
}
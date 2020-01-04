using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Users.Models;

namespace Crm.Apps.Clients.Users.Clients
{
    public interface IUsersClient
    {
        Task<List<UserGender>> GetGendersAsync(CancellationToken ct = default);

        Task<User> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<User>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<User>> GetPagedListAsync(Guid? accountId = default, string surname = default,
            string name = default, string patronymic = default, DateTime? minBirthDate = default,
            DateTime? maxBirthDate = default, UserGender? gender = default, bool? isLocked = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            bool? allAttributes = default, IDictionary<Guid, string> attributes = default,
            bool? allPermissions = default, List<Role> permissions = default, bool? allGroupIds = default,
            List<Guid> groupIds = default, int offset = default, int limit = 10, string sortBy = default,
            string orderBy = default, CancellationToken ct = default);

        Task<Guid> CreateAsync(User user, CancellationToken ct = default);

        Task UpdateAsync(User user, CancellationToken ct = default);

        Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
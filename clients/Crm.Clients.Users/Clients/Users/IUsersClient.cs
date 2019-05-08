using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Common.UserContext;

namespace Crm.Clients.Users.Clients.Users
{
    public interface IUsersClient
    {
        Task<ICollection<UserGender>> GetGendersAsync(CancellationToken ct = default);

        Task<User> GetAsync(Guid id, CancellationToken ct = default);

        Task<ICollection<User>> GetListAsync(ICollection<Guid> ids, CancellationToken ct = default);

        Task<ICollection<User>> GetPagedListAsync(Guid? accountId = default, string surname = default,
            string name = default, string patronymic = default, DateTime? minBirthDate = default,
            DateTime? maxBirthDate = default, UserGender? gender = default, bool? isLocked = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            bool? allAttributes = default, IDictionary<Guid, string> attributes = default,
            bool? allPermissions = default, ICollection<Permission> permissions = default, bool? allGroupIds = default,
            ICollection<Guid> groupIds = default, int offset = default, int limit = 10, string sortBy = default,
            string orderBy = default, CancellationToken ct = default);

        Task<Guid> CreateAsync(User user, CancellationToken ct = default);

        Task UpdateAsync(User user, CancellationToken ct = default);

        Task LockAsync(ICollection<Guid> ids, CancellationToken ct = default);

        Task UnlockAsync(ICollection<Guid> ids, CancellationToken ct = default);

        Task DeleteAsync(ICollection<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(ICollection<Guid> ids, CancellationToken ct = default);
    }
}
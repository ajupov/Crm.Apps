using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Common.UserContext;

namespace Crm.Apps.Areas.Users.Services
{
    public interface IUsersService
    {
        Task<User> GetAsync(Guid id, CancellationToken ct);

        Task<List<User>> GetListAsync(ICollection<Guid> ids, CancellationToken ct);

        Task<List<User>> GetPagedListAsync(Guid? accountId, string surname, string name, string patronymic,
            DateTime? minBirthDate, DateTime? maxBirthDate, UserGender? gender, bool? isLocked, bool? isDeleted,
            DateTime? minCreateDate, DateTime? maxCreateDate, bool? allAttributeIds, ICollection<Guid> attributeIds,
            bool? allPermissions, ICollection<Permission> permissions, bool? allGroupIds, ICollection<Guid> groupIds,
            int offset, int limit, string sortBy, string orderBy, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, User user, CancellationToken ct);

        Task UpdateAsync(Guid userId, User oldUser, User newUser, CancellationToken ct);

        Task LockAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct);

        Task UnlockAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct);

        Task DeleteAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct);
    }
}
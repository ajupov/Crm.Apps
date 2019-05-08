using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Common.Types;

namespace Crm.Apps.Areas.Users.Services
{
    public interface IUserAttributesService
    {
        Task<UserAttribute> GetAsync(Guid id, CancellationToken ct);

        Task<List<UserAttribute>> GetListAsync(ICollection<Guid> ids, CancellationToken ct);

        Task<List<UserAttribute>> GetPagedListAsync(Guid? accountId, AttributeType? type, string key, bool? isDeleted,
            DateTime? minCreateDate, DateTime? maxCreateDate, int offset, int limit, string sortBy, string orderBy,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, UserAttribute attribute, CancellationToken ct);

        Task UpdateAsync(Guid userId, UserAttribute oldAttribute, UserAttribute newAttribute, CancellationToken ct);

        Task DeleteAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct);
    }
}
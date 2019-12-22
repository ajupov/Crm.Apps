using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.RequestParameters;

namespace Crm.Apps.Areas.Users.Services
{
    public interface IUserAttributesService
    {
        Task<UserAttribute> GetAsync(Guid id, CancellationToken ct);

        Task<List<UserAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<UserAttribute>> GetPagedListAsync(UserAttributeGetPagedListRequestParameter request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, UserAttribute attribute, CancellationToken ct);

        Task UpdateAsync(Guid userId, UserAttribute oldAttribute, UserAttribute newAttribute, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
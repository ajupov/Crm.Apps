using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityAttributesService
    {
        Task<ActivityAttribute> GetAsync(Guid id, CancellationToken ct);

        Task<ActivityAttribute[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<ActivityAttribute[]> GetPagedListAsync(
            ActivityAttributeGetPagedListRequestParameter request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ActivityAttribute attribute, CancellationToken ct);

        Task UpdateAsync(
            Guid userId,
            ActivityAttribute oldAttribute,
            ActivityAttribute newAttribute,
            CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
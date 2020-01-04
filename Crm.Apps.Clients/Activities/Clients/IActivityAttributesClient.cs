using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Activities.Models;
using Crm.Apps.Clients.Activities.RequestParameters;

namespace Crm.Apps.Clients.Activities.Clients
{
    public interface IActivityAttributesClient
    {
        Task<Dictionary<string, AttributeType>> GetTypesAsync(CancellationToken ct = default);

        Task<ActivityAttribute> GetAsync(Guid id, CancellationToken ct = default);

        Task<ActivityAttribute[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<ActivityAttribute[]> GetPagedListAsync(
            ActivityAttributeGetPagedListRequest request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(ActivityAttributeCreateRequest request, CancellationToken ct = default);

        Task UpdateAsync(ActivityAttributeUpdateRequest request, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
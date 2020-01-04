using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;
using Crm.Common.Types;

namespace Crm.Clients.Activities.Clients
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
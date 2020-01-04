using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Activities.Models;
using Crm.Apps.Clients.Activities.RequestParameters;

namespace Crm.Apps.Clients.Activities.Clients
{
    public interface IActivityStatusesClient
    {
        Task<ActivityStatus> GetAsync(Guid id, CancellationToken ct = default);

        Task<ActivityStatus[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<ActivityStatus[]> GetPagedListAsync(
            ActivityStatusGetPagedListRequest request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(ActivityStatusCreateRequest request, CancellationToken ct = default);

        Task UpdateAsync(ActivityStatusUpdateRequest request, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Activities.Models;
using Crm.Apps.Clients.Activities.RequestParameters;

namespace Crm.Apps.Clients.Activities.Clients
{
    public interface IActivityTypesClient
    {
        Task<ActivityType> GetAsync(Guid id, CancellationToken ct = default);

        Task<ActivityType[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<ActivityType[]> GetPagedListAsync(
            ActivityTypeGetPagedListRequest request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(ActivityTypeCreateRequest request, CancellationToken ct = default);

        Task UpdateAsync(ActivityTypeUpdateRequest request, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
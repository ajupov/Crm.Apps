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

        Task<List<ActivityStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<ActivityStatus>> GetPagedListAsync(
            ActivityStatusGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(ActivityStatus status, CancellationToken ct = default);

        Task UpdateAsync(ActivityStatus status, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
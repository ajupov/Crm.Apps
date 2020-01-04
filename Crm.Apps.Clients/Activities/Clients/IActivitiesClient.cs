using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;

namespace Crm.Clients.Activities.Clients
{
    public interface IActivitiesClient
    {
        Task<Activity> GetAsync(Guid id, CancellationToken ct = default);

        Task<Activity[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<Activity[]> GetPagedListAsync(ActivityGetPagedListRequest request, CancellationToken ct = default);

        Task<Guid> CreateAsync(ActivityCreateRequest request, CancellationToken ct = default);

        Task UpdateAsync(ActivityUpdateRequest request, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
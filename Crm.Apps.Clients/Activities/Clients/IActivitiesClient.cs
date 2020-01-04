using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Activities.Models;
using Crm.Apps.Clients.Activities.RequestParameters;

namespace Crm.Apps.Clients.Activities.Clients
{
    public interface IActivitiesClient
    {
        Task<Activity> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Activity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Activity>> GetPagedListAsync(
            ActivityGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(Activity activity, CancellationToken ct = default);

        Task UpdateAsync(Activity activity, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
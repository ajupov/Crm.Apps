using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Activities.Models;
using Crm.Apps.v1.Clients.Activities.RequestParameters;

namespace Crm.Apps.v1.Clients.Activities.Clients
{
    public interface IActivitiesClient
    {
        Task<Activity> GetAsync(string accessToken, Guid id, CancellationToken ct = default);

        Task<List<Activity>> GetListAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Activity>> GetPagedListAsync(
            string accessToken,
            ActivityGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(string accessToken, Activity activity, CancellationToken ct = default);

        Task UpdateAsync(string accessToken, Activity activity, CancellationToken ct = default);

        Task DeleteAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
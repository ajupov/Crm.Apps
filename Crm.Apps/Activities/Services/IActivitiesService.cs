using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.v1.Models;
using Crm.Apps.Activities.v1.RequestParameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivitiesService
    {
        Task<Activity> GetAsync(Guid id, CancellationToken ct);

        Task<List<Activity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<Activity>> GetPagedListAsync(ActivityGetPagedListRequestParameter request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Activity activity, CancellationToken ct);

        Task UpdateAsync(Guid userId, Activity oldActivity, Activity newActivity, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
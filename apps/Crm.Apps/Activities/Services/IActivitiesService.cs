using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivitiesService
    {
        Task<Activity> GetAsync(Guid id, CancellationToken ct);

        Task<Activity[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<Activity[]> GetPagedListAsync(ActivityGetPagedListRequestParameter request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Activity activity, CancellationToken ct);

        Task UpdateAsync(Guid userId, Activity oldActivity, Activity newActivity, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
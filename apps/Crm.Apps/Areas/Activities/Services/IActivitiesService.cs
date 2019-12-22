using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;

namespace Crm.Apps.Areas.Activities.Services
{
    public interface IActivitiesService
    {
        Task<Activity> GetAsync(Guid id, CancellationToken ct);

        Task<Activity[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<Activity[]> GetPagedListAsync(ActivityGetPagedListRequest request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ActivityCreateRequest request, CancellationToken ct);

        Task UpdateAsync(Guid userId, Activity activity, ActivityUpdateRequest request, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
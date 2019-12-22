using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;

namespace Crm.Apps.Areas.Activities.Services
{
    public interface IActivityTypesService
    {
        Task<ActivityType> GetAsync(Guid id, CancellationToken ct);

        Task<ActivityType[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<ActivityType[]> GetPagedListAsync(ActivityTypeGetPagedListRequest request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ActivityTypeCreateRequest request, CancellationToken ct);

        Task UpdateAsync(Guid userId, ActivityType type, ActivityTypeUpdateRequest request, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
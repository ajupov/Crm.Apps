using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.V1.Requests;
using Crm.Apps.Activities.V1.Responses;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityTypesService
    {
        Task<ActivityType> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<ActivityType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<ActivityTypeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            ActivityTypeGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ActivityType type, CancellationToken ct);

        Task UpdateAsync(Guid userId, ActivityType oldType, ActivityType newType, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}

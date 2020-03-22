using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.v1.Requests;
using Crm.Apps.Activities.v1.Responses;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityStatusesService
    {
        Task<ActivityStatus> GetAsync(Guid id, CancellationToken ct);

        Task<List<ActivityStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<ActivityStatusGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            ActivityStatusGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ActivityStatus status, CancellationToken ct);

        Task UpdateAsync(Guid userId, ActivityStatus oldStatus, ActivityStatus newStatus, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
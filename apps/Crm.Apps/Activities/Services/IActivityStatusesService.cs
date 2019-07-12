using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Parameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityStatusesService
    {
        Task<ActivityStatus> GetAsync(Guid id, CancellationToken ct);

        Task<List<ActivityStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<ActivityStatus>> GetPagedListAsync(ActivityStatusGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ActivityStatus status, CancellationToken ct);

        Task UpdateAsync(Guid userId, ActivityStatus oldStatus, ActivityStatus newStatus, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
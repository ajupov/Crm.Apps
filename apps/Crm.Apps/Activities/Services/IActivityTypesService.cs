using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Parameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityTypesService
    {
        Task<ActivityType> GetAsync(Guid id, CancellationToken ct);

        Task<List<ActivityType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<ActivityType>> GetPagedListAsync(ActivityTypeGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ActivityType status, CancellationToken ct);

        Task UpdateAsync(Guid userId, ActivityType oldType, ActivityType newType, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
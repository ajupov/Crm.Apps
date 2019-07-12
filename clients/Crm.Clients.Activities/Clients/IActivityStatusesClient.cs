using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;

namespace Crm.Clients.Activities.Clients
{
    public interface IActivityStatusesClient
    {
        Task<ActivityStatus> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<ActivityStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<ActivityStatus>> GetPagedListAsync(Guid? accountId = default, string name = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(ActivityStatus status, CancellationToken ct = default);

        Task UpdateAsync(ActivityStatus status, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
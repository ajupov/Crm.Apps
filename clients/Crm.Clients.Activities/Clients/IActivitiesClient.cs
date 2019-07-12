using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;

namespace Crm.Clients.Activities.Clients
{
    public interface IActivitiesClient
    {
        Task<Activity> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Activity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Activity>> GetPagedListAsync(Guid? accountId = default, string name = default,
            string description = default, string result = default, DateTime? minStartDateTime = default,
            DateTime? maxStartDateTime = default, DateTime? minEndDateTime = default,
            DateTime? maxEndDateTime = default, DateTime? minDeadLineDateTime = default,
            DateTime? maxDeadLineDateTime = default, bool isDeleted = default, DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default, bool? allAttributes = default,
            IDictionary<Guid, string> attributes = default, List<Guid> typeIds = default,
            List<Guid> statusIds = default, List<Guid> leadIds = default, List<Guid> companyIds = default,
            List<Guid> contactIds = default, List<Guid> dealIds = default, List<Guid> createUserIds = default,
            List<Guid> responsibleUserIds = default, int offset = default, int limit = 10, string sortBy = default,
            string orderBy = default, CancellationToken ct = default);

        Task<Guid> CreateAsync(Activity activity, CancellationToken ct = default);

        Task UpdateAsync(Activity activity, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
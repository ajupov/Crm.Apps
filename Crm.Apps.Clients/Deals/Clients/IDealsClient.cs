using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Deals.Models;

namespace Crm.Clients.Deals.Clients
{
    public interface IDealsClient
    {
        Task<Deal> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Deal>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Deal>> GetPagedListAsync(Guid? accountId = default, string name = default,
            DateTime? minStartDateTime = default, DateTime? maxStartDateTime = default,
            DateTime? minEndDateTime = default, DateTime? maxEndDateTime = default, decimal? minSum = default,
            decimal? maxSum = default, decimal? minSumWithoutDiscount = default, decimal? maxSumWithoutDiscount = default,
            byte? minFinishProbability = default, byte? maxFinishProbability = default, bool isDeleted = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, List<Guid> typeIds = default,
            List<Guid> statusIds = default, List<Guid> companyIds = default, List<Guid> contactIds = default,
            List<Guid> createUserIds = default, List<Guid> responsibleUserIds = default, bool? allAttributes = default,
            IDictionary<Guid, string> attributes = default, List<Guid> positionsProductIds = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(Deal deal, CancellationToken ct = default);

        Task UpdateAsync(Deal deal, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}
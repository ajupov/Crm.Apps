using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;

namespace Crm.Apps.Areas.Deals.Services
{
    public interface IDealsService
    {
        Task<Deal> GetAsync(Guid id, CancellationToken ct);

        Task<List<Deal>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<Deal>> GetPagedListAsync(DealGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Deal deal, CancellationToken ct);

        Task UpdateAsync(Guid userId, Deal oldDeal, Deal newDeal, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
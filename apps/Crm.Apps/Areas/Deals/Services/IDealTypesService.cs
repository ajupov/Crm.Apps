using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.RequestParameters;

namespace Crm.Apps.Areas.Deals.Services
{
    public interface IDealTypesService
    {
        Task<DealType> GetAsync(Guid id, CancellationToken ct);

        Task<List<DealType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<DealType>> GetPagedListAsync(DealTypeGetPagedListRequestParameter request, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, DealType type, CancellationToken ct);

        Task UpdateAsync(Guid userId, DealType oldType, DealType newType, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
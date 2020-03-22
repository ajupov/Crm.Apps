using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.v1.Requests;
using Crm.Apps.Deals.v1.Responses;

namespace Crm.Apps.Deals.Services
{
    public interface IDealTypesService
    {
        Task<DealType> GetAsync(Guid id, CancellationToken ct);

        Task<List<DealType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<DealTypeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            DealTypeGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, DealType type, CancellationToken ct);

        Task UpdateAsync(Guid userId, DealType oldType, DealType newType, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
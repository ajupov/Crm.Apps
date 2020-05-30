using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.V1.Requests;
using Crm.Apps.Deals.V1.Responses;

namespace Crm.Apps.Deals.Services
{
    public interface IDealAttributesService
    {
        Task<DealAttribute> GetAsync(Guid id, CancellationToken ct);

        Task<List<DealAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<DealAttributeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            DealAttributeGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, DealAttribute attribute, CancellationToken ct);

        Task UpdateAsync(Guid userId, DealAttribute oldAttribute, DealAttribute newAttribute, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}

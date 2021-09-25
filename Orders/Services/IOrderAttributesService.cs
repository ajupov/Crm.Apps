using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Orders.Models;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;

namespace Crm.Apps.Orders.Services
{
    public interface IOrderAttributesService
    {
        Task<OrderAttribute> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<OrderAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<OrderAttributeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            OrderAttributeGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, OrderAttribute attribute, CancellationToken ct);

        Task UpdateAsync(Guid userId, OrderAttribute oldAttribute, OrderAttribute newAttribute, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}

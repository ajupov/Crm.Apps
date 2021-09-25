using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Orders.Models;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;

namespace Crm.Apps.Orders.Services
{
    public interface IOrderStatusesService
    {
        Task<OrderStatus> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<OrderStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<OrderStatusGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            OrderStatusGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, OrderStatus status, CancellationToken ct);

        Task UpdateAsync(Guid userId, OrderStatus oldStatus, OrderStatus newStatus, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}

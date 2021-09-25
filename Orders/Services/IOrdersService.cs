using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Orders.Models;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;

namespace Crm.Apps.Orders.Services
{
    public interface IOrdersService
    {
        Task<Order> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<Order>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<OrderGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            OrderGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Order order, CancellationToken ct);

        Task UpdateAsync(Guid userId, Order oldOrder, Order newOrder, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}

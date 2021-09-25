using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Orders.Models;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;

namespace Crm.Apps.Orders.Services
{
    public interface IOrderTypesService
    {
        Task<OrderType> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<OrderType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<OrderTypeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            OrderTypeGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, OrderType type, CancellationToken ct);

        Task UpdateAsync(Guid userId, OrderType oldType, OrderType newType, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}

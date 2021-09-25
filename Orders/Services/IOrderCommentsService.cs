using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Orders.Models;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;

namespace Crm.Apps.Orders.Services
{
    public interface IOrderCommentsService
    {
        Task<OrderCommentGetPagedListResponse> GetPagedListAsync(
            OrderCommentGetPagedListRequest request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, OrderComment comment, CancellationToken ct);
    }
}

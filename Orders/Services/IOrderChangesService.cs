using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;

namespace Crm.Apps.Orders.Services
{
    public interface IOrderChangesService
    {
        Task<OrderChangeGetPagedListResponse> GetPagedListAsync(
            OrderChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}

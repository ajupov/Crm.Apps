using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;

namespace Crm.Apps.Orders.Services
{
    public interface IOrderAttributeChangesService
    {
        Task<OrderAttributeChangeGetPagedListResponse> GetPagedListAsync(
            OrderAttributeChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}

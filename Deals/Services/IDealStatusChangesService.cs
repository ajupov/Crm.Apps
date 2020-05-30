using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.V1.Requests;
using Crm.Apps.Deals.V1.Responses;

namespace Crm.Apps.Deals.Services
{
    public interface IDealStatusChangesService
    {
        Task<DealStatusChangeGetPagedListResponse> GetPagedListAsync(
            DealStatusChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}

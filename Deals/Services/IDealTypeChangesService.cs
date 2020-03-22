using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.v1.Requests;
using Crm.Apps.Deals.v1.Responses;

namespace Crm.Apps.Deals.Services
{
    public interface IDealTypeChangesService
    {
        Task<DealTypeChangeGetPagedListResponse> GetPagedListAsync(
            DealTypeChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
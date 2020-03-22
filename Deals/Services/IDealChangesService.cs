using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.v1.Requests;
using Crm.Apps.Deals.v1.Responses;

namespace Crm.Apps.Deals.Services
{
    public interface IDealChangesService
    {
        Task<DealChangeGetPagedListResponse> GetPagedListAsync(
            DealChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
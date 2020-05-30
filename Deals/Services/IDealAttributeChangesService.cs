using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.V1.Requests;
using Crm.Apps.Deals.V1.Responses;

namespace Crm.Apps.Deals.Services
{
    public interface IDealAttributeChangesService
    {
        Task<DealAttributeChangeGetPagedListResponse> GetPagedListAsync(
            DealAttributeChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}

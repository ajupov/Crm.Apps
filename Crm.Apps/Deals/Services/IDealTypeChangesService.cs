using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.v1.Models;
using Crm.Apps.Deals.v1.RequestParameters;

namespace Crm.Apps.Deals.Services
{
    public interface IDealTypeChangesService
    {
        Task<List<DealTypeChange>> GetPagedListAsync(
            DealTypeChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}
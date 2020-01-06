using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.RequestParameters;

namespace Crm.Apps.Clients.Deals.Clients
{
    public interface IDealTypeChangesClient
    {
        Task<List<DealTypeChange>> GetPagedListAsync(
            DealTypeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}
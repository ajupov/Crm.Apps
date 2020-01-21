using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Deals.Models;
using Crm.Apps.v1.Clients.Deals.RequestParameters;

namespace Crm.Apps.v1.Clients.Deals.Clients
{
    public interface IDealChangesClient
    {
        Task<List<DealChange>> GetPagedListAsync(
            DealChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}
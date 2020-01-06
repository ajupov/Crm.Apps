using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.RequestParameters;

namespace Crm.Apps.Clients.Deals.Clients
{
    public interface IDealStatusChangesClient
    {
        Task<List<DealStatusChange>> GetPagedListAsync(
            DealStatusChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}
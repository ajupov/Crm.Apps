using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Leads.Models;
using Crm.Apps.v1.Clients.Leads.RequestParameters;

namespace Crm.Apps.v1.Clients.Leads.Clients
{
    public interface ILeadChangesClient
    {
        Task<List<LeadChange>> GetPagedListAsync(
            string accessToken,
            LeadChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}
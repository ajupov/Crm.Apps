using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Leads.Models;
using Crm.Apps.v1.Clients.Leads.RequestParameters;

namespace Crm.Apps.v1.Clients.Leads.Clients
{
    public interface ILeadSourceChangesClient
    {
        Task<List<LeadSourceChange>> GetPagedListAsync(
            string accessToken,
            LeadSourceChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}
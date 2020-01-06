using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Leads.Models;
using Crm.Apps.Clients.Leads.RequestParameters;

namespace Crm.Apps.Clients.Leads.Clients
{
    public interface ILeadSourceChangesClient
    {
        Task<List<LeadSourceChange>> GetPagedListAsync(
            LeadSourceChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}
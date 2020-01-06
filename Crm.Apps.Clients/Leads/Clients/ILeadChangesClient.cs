using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Leads.Models;
using Crm.Apps.Clients.Leads.RequestParameters;

namespace Crm.Apps.Clients.Leads.Clients
{
    public interface ILeadChangesClient
    {
        Task<List<LeadChange>> GetPagedListAsync(
            LeadChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}
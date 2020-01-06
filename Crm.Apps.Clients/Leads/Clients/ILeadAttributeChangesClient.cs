using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Leads.Models;
using Crm.Apps.Clients.Leads.RequestParameters;

namespace Crm.Apps.Clients.Leads.Clients
{
    public interface ILeadAttributeChangesClient
    {
        Task<List<LeadAttributeChange>> GetPagedListAsync(
            LeadAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}
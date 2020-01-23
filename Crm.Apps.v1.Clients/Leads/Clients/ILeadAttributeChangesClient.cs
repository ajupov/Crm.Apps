using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Leads.Models;
using Crm.Apps.v1.Clients.Leads.RequestParameters;

namespace Crm.Apps.v1.Clients.Leads.Clients
{
    public interface ILeadAttributeChangesClient
    {
        Task<List<LeadAttributeChange>> GetPagedListAsync(
            string accessToken,
            LeadAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}
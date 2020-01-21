using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Leads.Models;
using Crm.Apps.v1.Clients.Leads.RequestParameters;

namespace Crm.Apps.v1.Clients.Leads.Clients
{
    public interface ILeadCommentsClient
    {
        Task<List<LeadComment>> GetPagedListAsync(
            LeadCommentGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task CreateAsync(LeadComment comment, CancellationToken ct = default);
    }
}
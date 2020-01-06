using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Leads.Models;
using Crm.Apps.Clients.Leads.RequestParameters;

namespace Crm.Apps.Clients.Leads.Clients
{
    public interface ILeadCommentsClient
    {
        Task<List<LeadComment>> GetPagedListAsync(
            LeadCommentGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task CreateAsync(LeadComment comment, CancellationToken ct = default);
    }
}
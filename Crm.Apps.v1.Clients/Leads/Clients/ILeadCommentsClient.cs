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
            string accessToken,
            LeadCommentGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task CreateAsync(string accessToken, LeadComment comment, CancellationToken ct = default);
    }
}
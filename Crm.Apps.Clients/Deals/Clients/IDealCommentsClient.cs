using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.RequestParameters;

namespace Crm.Apps.Clients.Deals.Clients
{
    public interface IDealCommentsClient
    {
        Task<List<DealComment>> GetPagedListAsync(
            DealCommentGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task CreateAsync(DealComment comment, CancellationToken ct = default);
    }
}
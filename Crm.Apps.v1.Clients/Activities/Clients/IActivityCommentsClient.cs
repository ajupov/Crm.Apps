using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Activities.Models;
using Crm.Apps.v1.Clients.Activities.RequestParameters;

namespace Crm.Apps.v1.Clients.Activities.Clients
{
    public interface IActivityCommentsClient
    {
        Task<List<ActivityComment>> GetPagedListAsync(
            string accessToken,
            ActivityCommentGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task CreateAsync(string accessToken, ActivityComment comment, CancellationToken ct = default);
    }
}
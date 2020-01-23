using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Activities.Models;
using Crm.Apps.v1.Clients.Activities.RequestParameters;

namespace Crm.Apps.v1.Clients.Activities.Clients
{
    public interface IActivityAttributeChangesClient
    {
        Task<List<ActivityAttributeChange>> GetPagedListAsync(
            string accessToken,
            ActivityAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}
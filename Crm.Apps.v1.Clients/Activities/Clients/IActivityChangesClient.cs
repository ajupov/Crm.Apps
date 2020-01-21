using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Activities.Models;
using Crm.Apps.v1.Clients.Activities.RequestParameters;

namespace Crm.Apps.v1.Clients.Activities.Clients
{
    public interface IActivityChangesClient
    {
        Task<List<ActivityChange>> GetPagedListAsync(
            ActivityChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Activities.Models;
using Crm.Apps.Clients.Activities.RequestParameters;

namespace Crm.Apps.Clients.Activities.Clients
{
    public interface IActivityChangesClient
    {
        Task<List<ActivityChange>> GetPagedListAsync(
            ActivityChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}
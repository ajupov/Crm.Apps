using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;

namespace Crm.Clients.Activities.Clients
{
    public interface IActivityStatusChangesClient
    {
        Task<ActivityStatusChange[]> GetPagedListAsync(
            ActivityStatusChangeGetPagedListRequest request,
            CancellationToken ct = default);
    }
}
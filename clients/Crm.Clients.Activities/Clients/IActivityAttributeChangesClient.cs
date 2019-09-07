using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;

namespace Crm.Clients.Activities.Clients
{
    public interface IActivityAttributeChangesClient
    {
        Task<List<ActivityAttributeChange>> GetPagedListAsync(
            ActivityAttributeChangeGetPagedListRequest request,
            CancellationToken ct = default);
    }
}
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;

namespace Crm.Clients.Activities.Clients
{
    public interface IActivityTypeChangesClient
    {
        Task<ActivityTypeChange[]> GetPagedListAsync(
            ActivityTypeChangeGetPagedListRequest request,
            CancellationToken ct = default);
    }
}
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;

namespace Crm.Clients.Activities.Clients
{
    public interface IActivityChangesClient
    {
        Task<ActivityChange[]> GetPagedListAsync(
            ActivityChangeGetPagedListRequest request,
            CancellationToken ct = default);
    }
}
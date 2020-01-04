using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityStatusChangesService
    {
        Task<ActivityStatusChange[]> GetPagedListAsync(
            ActivityStatusChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}
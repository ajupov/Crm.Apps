using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityChangesService
    {
        Task<ActivityChange[]> GetPagedListAsync(ActivityChangeGetPagedListRequest request, CancellationToken ct);
    }
}
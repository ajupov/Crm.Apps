using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;

namespace Crm.Apps.Areas.Activities.Services
{
    public interface IActivityChangesService
    {
        Task<ActivityChange[]> GetPagedListAsync(ActivityChangeGetPagedListRequest request, CancellationToken ct);
    }
}
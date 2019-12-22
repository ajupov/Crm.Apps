using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;

namespace Crm.Apps.Areas.Activities.Services
{
    public interface IActivityTypeChangesService
    {
        Task<ActivityTypeChange[]> GetPagedListAsync(
            ActivityTypeChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
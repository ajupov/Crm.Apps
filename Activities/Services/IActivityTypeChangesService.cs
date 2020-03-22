using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.v1.Requests;
using Crm.Apps.Activities.v1.Responses;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityTypeChangesService
    {
        Task<ActivityTypeChangeGetPagedListResponse> GetPagedListAsync(
            ActivityTypeChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
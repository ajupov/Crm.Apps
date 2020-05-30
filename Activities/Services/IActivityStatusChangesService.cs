using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.V1.Requests;
using Crm.Apps.Activities.V1.Responses;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityStatusChangesService
    {
        Task<ActivityStatusChangeGetPagedListResponse> GetPagedListAsync(
            ActivityStatusChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}

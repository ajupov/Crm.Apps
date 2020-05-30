using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.V1.Requests;
using Crm.Apps.Activities.V1.Responses;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityChangesService
    {
        Task<ActivityChangeGetPagedListResponse> GetPagedListAsync(
            ActivityChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}

using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Tasks.V1.Requests;
using Crm.Apps.Tasks.V1.Responses;

namespace Crm.Apps.Tasks.Services
{
    public interface ITaskStatusChangesService
    {
        Task<TaskStatusChangeGetPagedListResponse> GetPagedListAsync(
            TaskStatusChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}

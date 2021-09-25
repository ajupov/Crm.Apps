using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Tasks.Models;
using Crm.Apps.Tasks.V1.Requests;
using Crm.Apps.Tasks.V1.Responses;
using Task = System.Threading.Tasks.Task;

namespace Crm.Apps.Tasks.Services
{
    public interface ITaskCommentsService
    {
        Task<TaskCommentGetPagedListResponse> GetPagedListAsync(
            TaskCommentGetPagedListRequest request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, TaskComment comment, CancellationToken ct);
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Tasks.Models;
using Crm.Apps.Tasks.V1.Requests;
using Crm.Apps.Tasks.V1.Responses;
using Task = System.Threading.Tasks.Task;
using TaskStatus = Crm.Apps.Tasks.Models.TaskStatus;

namespace Crm.Apps.Tasks.Services
{
    public interface ITaskStatusesService
    {
        Task<TaskStatus> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<TaskStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<TaskStatusGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            TaskStatusGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, TaskStatus status, CancellationToken ct);

        Task UpdateAsync(Guid userId, TaskStatus oldStatus, TaskStatus newStatus, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Tasks.V1.Requests;
using Crm.Apps.Tasks.V1.Responses;
using CrmTask = Crm.Apps.Tasks.Models.Task;

namespace Crm.Apps.Tasks.Services
{
    public interface ITasksService
    {
        Task<CrmTask> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<CrmTask>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<TaskGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            TaskGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, CrmTask task, CancellationToken ct);

        Task UpdateAsync(Guid userId, CrmTask oldTask, CrmTask newTask, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}

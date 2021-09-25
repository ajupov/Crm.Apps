using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Tasks.Models;
using Crm.Apps.Tasks.V1.Requests;
using Crm.Apps.Tasks.V1.Responses;
using Task = System.Threading.Tasks.Task;

namespace Crm.Apps.Tasks.Services
{
    public interface ITaskTypesService
    {
        Task<TaskType> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<TaskType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<TaskTypeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            TaskTypeGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, TaskType type, CancellationToken ct);

        Task UpdateAsync(Guid userId, TaskType oldType, TaskType newType, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}

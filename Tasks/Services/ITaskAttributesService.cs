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
    public interface ITaskAttributesService
    {
        Task<TaskAttribute> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<TaskAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<TaskAttributeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            TaskAttributeGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, TaskAttribute attribute, CancellationToken ct);

        Task UpdateAsync(
            Guid userId,
            TaskAttribute oldAttribute,
            TaskAttribute newAttribute,
            CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}

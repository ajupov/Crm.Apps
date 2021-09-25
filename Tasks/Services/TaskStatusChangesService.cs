using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Tasks.Storages;
using Crm.Apps.Tasks.V1.Requests;
using Crm.Apps.Tasks.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Tasks.Services
{
    public class TaskStatusChangesService : ITaskStatusChangesService
    {
        private readonly TasksStorage _storage;

        public TaskStatusChangesService(TasksStorage storage)
        {
            _storage = storage;
        }

        public async Task<TaskStatusChangeGetPagedListResponse> GetPagedListAsync(
            TaskStatusChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.TaskStatusChanges
                .AsNoTracking()
                .Where(x =>
                    (request.StatusId.IsEmpty() || x.StatusId == request.StatusId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new TaskStatusChangeGetPagedListResponse
            {
                TotalCount = await changes
                    .CountAsync(ct),
                Changes = await changes
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }
    }
}

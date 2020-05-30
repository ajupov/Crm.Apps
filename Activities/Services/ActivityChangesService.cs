using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Activities.Storages;
using Crm.Apps.Activities.V1.Requests;
using Crm.Apps.Activities.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Activities.Services
{
    public class ActivityChangesService : IActivityChangesService
    {
        private readonly ActivitiesStorage _storage;

        public ActivityChangesService(ActivitiesStorage storage)
        {
            _storage = storage;
        }

        public async Task<ActivityChangeGetPagedListResponse> GetPagedListAsync(
            ActivityChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.ActivityChanges
                .Where(x =>
                    (request.ActivityId.IsEmpty() || x.ActivityId == request.ActivityId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new ActivityChangeGetPagedListResponse
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

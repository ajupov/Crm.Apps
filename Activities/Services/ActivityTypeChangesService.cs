using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Activities.Storages;
using Crm.Apps.Activities.v1.Requests;
using Crm.Apps.Activities.v1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Activities.Services
{
    public class ActivityTypeChangesService : IActivityTypeChangesService
    {
        private readonly ActivitiesStorage _storage;

        public ActivityTypeChangesService(ActivitiesStorage storage)
        {
            _storage = storage;
        }

        public async Task<ActivityTypeChangeGetPagedListResponse> GetPagedListAsync(
            ActivityTypeChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.ActivityTypeChanges
                .Where(x =>
                    (request.TypeId.IsEmpty() || x.TypeId == request.TypeId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new ActivityTypeChangeGetPagedListResponse
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
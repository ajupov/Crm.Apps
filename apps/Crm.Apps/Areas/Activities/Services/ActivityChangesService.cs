using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;
using Crm.Apps.Areas.Activities.Storages;

namespace Crm.Apps.Areas.Activities.Services
{
    public class ActivityChangesService : IActivityChangesService
    {
        private readonly ActivitiesStorage _activitiesStorage;

        public ActivityChangesService(ActivitiesStorage activitiesStorage)
        {
            _activitiesStorage = activitiesStorage;
        }

        public Task<ActivityChange[]> GetPagedListAsync(
            ActivityChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            return _activitiesStorage.ActivityChanges
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.ActivityId.IsEmpty() || x.ActivityId == request.ActivityId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArrayAsync(ct);
        }
    }
}
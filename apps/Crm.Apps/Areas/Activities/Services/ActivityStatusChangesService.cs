using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;
using Crm.Apps.Areas.Activities.Storages;

namespace Crm.Apps.Areas.Activities.Services
{
    public class ActivityStatusChangesService : IActivityStatusChangesService
    {
        private readonly ActivitiesStorage _activitiesStorage;

        public ActivityStatusChangesService(ActivitiesStorage activitiesStorage)
        {
            _activitiesStorage = activitiesStorage;
        }

        public Task<ActivityStatusChange[]> GetPagedListAsync(
            ActivityStatusChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            return _activitiesStorage.ActivityStatusChanges
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.StatusId.IsEmpty() || x.StatusId == request.StatusId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArrayAsync(ct);
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;
using Crm.Apps.Areas.Activities.Storages;

namespace Crm.Apps.Areas.Activities.Services
{
    public class ActivityAttributeChangesService : IActivityAttributeChangesService
    {
        private readonly ActivitiesStorage _activitiesStorage;

        public ActivityAttributeChangesService(ActivitiesStorage activitiesStorage)
        {
            _activitiesStorage = activitiesStorage;
        }

        public Task<ActivityAttributeChange[]> GetPagedListAsync(
            ActivityAttributeChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            return _activitiesStorage.ActivityAttributeChanges
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.AttributeId.IsEmpty() || x.AttributeId == request.AttributeId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArrayAsync(ct);
        }
    }
}
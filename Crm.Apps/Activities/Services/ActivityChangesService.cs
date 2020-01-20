using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Activities.Services
{
    public class ActivityChangesService : IActivityChangesService
    {
        private readonly ActivitiesStorage _activitiesStorage;

        public ActivityChangesService(ActivitiesStorage activitiesStorage)
        {
            _activitiesStorage = activitiesStorage;
        }

        public Task<List<ActivityChange>> GetPagedListAsync(
            ActivityChangeGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _activitiesStorage.ActivityChanges
                .AsNoTracking()
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.ActivityId.IsEmpty() || x.ActivityId == request.ActivityId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Helpers;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Parameters;
using Crm.Apps.Activities.Storages;
using Crm.Utils.Guid;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Activities.Services
{
    public class ActivityStatusChangesService : IActivityStatusChangesService
    {
        private readonly ActivitiesStorage _storage;

        public ActivityStatusChangesService(ActivitiesStorage storage)
        {
            _storage = storage;
        }

        public Task<List<ActivityStatusChange>> GetPagedListAsync(ActivityStatusChangeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.ActivityStatusChanges.Where(x =>
                    (parameter.ChangerUserId.IsEmpty() || x.ChangerUserId == parameter.ChangerUserId) &&
                    (parameter.StatusId.IsEmpty() || x.StatusId == parameter.StatusId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}
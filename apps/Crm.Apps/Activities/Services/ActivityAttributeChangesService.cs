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
    public class ActivityAttributeChangesService : IActivityAttributeChangesService
    {
        private readonly ActivitiesStorage _storage;

        public ActivityAttributeChangesService(ActivitiesStorage storage)
        {
            _storage = storage;
        }

        public Task<List<ActivityAttributeChange>> GetPagedListAsync(
            ActivityAttributeChangeGetPagedListParameter parameter, CancellationToken ct)
        {
            return _storage.ActivityAttributeChanges.Where(x =>
                    (parameter.ChangerUserId.IsEmpty() || x.ChangerUserId == parameter.ChangerUserId) &&
                    (parameter.AttributeId.IsEmpty() || x.AttributeId == parameter.AttributeId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}
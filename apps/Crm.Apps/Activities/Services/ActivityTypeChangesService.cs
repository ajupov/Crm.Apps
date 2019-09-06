﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Storages;
using Crm.Utils.Guid;
using Crm.Utils.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Activities.Services
{
    public class ActivityTypeChangesService : IActivityTypeChangesService
    {
        private readonly ActivitiesStorage _activitiesStorage;

        public ActivityTypeChangesService(ActivitiesStorage activitiesStorage)
        {
            _activitiesStorage = activitiesStorage;
        }

        public Task<ActivityTypeChange[]> GetPagedListAsync(
            ActivityTypeChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            return _activitiesStorage.ActivityTypeChanges
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.TypeId.IsEmpty() || x.TypeId == request.TypeId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArrayAsync(ct);
        }
    }
}
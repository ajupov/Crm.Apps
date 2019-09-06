using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Helpers;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Storages;
using Crm.Utils.Guid;
using Crm.Utils.Sorting;
using Crm.Utils.String;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Activities.Services
{
    public class ActivityStatusesService : IActivityStatusesService
    {
        private readonly ActivitiesStorage _activitiesStorage;

        public ActivityStatusesService(ActivitiesStorage activitiesStorage)
        {
            _activitiesStorage = activitiesStorage;
        }

        public Task<ActivityStatus> GetAsync(Guid id, CancellationToken ct)
        {
            return _activitiesStorage.ActivityStatuses
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<ActivityStatus[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _activitiesStorage.ActivityStatuses
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync(ct);
        }

        public Task<ActivityStatus[]> GetPagedListAsync(
            ActivityStatusGetPagedListRequest request,
            CancellationToken ct)
        {
            return _activitiesStorage.ActivityStatuses
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArrayAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, ActivityStatusCreateRequest request, CancellationToken ct)
        {
            var status = new ActivityStatus();
            var change = status.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = request.AccountId;
                x.Name = request.Name;
                x.IsDeleted = request.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _activitiesStorage.AddAsync(status, ct);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            ActivityStatus status,
            ActivityStatusUpdateRequest request,
            CancellationToken ct)
        {
            var change = status.WithUpdateLog(userId, x =>
            {
                x.Name = request.Name;
                x.IsDeleted = request.IsDeleted;
            });

            _activitiesStorage.Update(status);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityStatusChange>();

            await _activitiesStorage.ActivityStatuses
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityStatusChange>();

            await _activitiesStorage.ActivityStatuses
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }
    }
}
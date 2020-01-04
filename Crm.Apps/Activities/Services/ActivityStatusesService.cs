using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Activities.Helpers;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Storages;
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
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<ActivityStatus[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _activitiesStorage.ActivityStatuses
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync(ct);
        }

        public Task<ActivityStatus[]> GetPagedListAsync(
            ActivityStatusGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _activitiesStorage.ActivityStatuses
                .AsNoTracking()
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArrayAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, ActivityStatus status, CancellationToken ct)
        {
            var newStatus = new ActivityStatus();
            var change = newStatus.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = status.AccountId;
                x.Name = status.Name;
                x.IsDeleted = status.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _activitiesStorage.AddAsync(newStatus, ct);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            ActivityStatus oldStatus,
            ActivityStatus newStatus,
            CancellationToken ct)
        {
            var change = oldStatus.WithUpdateLog(userId, x =>
            {
                x.Name = newStatus.Name;
                x.IsDeleted = newStatus.IsDeleted;
            });

            _activitiesStorage.Update(oldStatus);
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
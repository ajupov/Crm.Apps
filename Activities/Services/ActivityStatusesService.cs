using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Activities.Helpers;
using Crm.Apps.Activities.Storages;
using Crm.Apps.Activities.v1.Models;
using Crm.Apps.Activities.v1.RequestParameters;
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

        public Task<List<ActivityStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _activitiesStorage.ActivityStatuses
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public Task<List<ActivityStatus>> GetPagedListAsync(
            ActivityStatusGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _activitiesStorage.ActivityStatuses
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (!request.IsFinish.HasValue || x.IsFinish == request.IsFinish) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, ActivityStatus status, CancellationToken ct)
        {
            var newStatus = new ActivityStatus();
            var change = newStatus.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = status.AccountId;
                x.Name = status.Name;
                x.IsFinish = status.IsFinish;
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
                x.IsFinish = newStatus.IsFinish;
                x.IsDeleted = newStatus.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
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
                .ForEachAsync(x => changes.Add(x.WithUpdateLog(userId, s =>
                {
                    s.IsDeleted = true;
                    s.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityStatusChange>();

            await _activitiesStorage.ActivityStatuses
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.WithUpdateLog(userId, s =>
                {
                    s.IsDeleted = false;
                    s.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }
    }
}
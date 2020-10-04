using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Activities.Helpers;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Storages;
using Crm.Apps.Activities.V1.Requests;
using Crm.Apps.Activities.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Activities.Services
{
    public class ActivityStatusesService : IActivityStatusesService
    {
        private readonly ActivitiesStorage _storage;

        public ActivityStatusesService(ActivitiesStorage storage)
        {
            _storage = storage;
        }

        public Task<ActivityStatus> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.ActivityStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<ActivityStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.ActivityStatuses
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<ActivityStatusGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            ActivityStatusGetPagedListRequest request,
            CancellationToken ct)
        {
            var statuses = _storage.ActivityStatuses
                .AsNoTracking()
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Name.IsEmpty() || EF.Functions.ILike(x.Name, $"{request.Name}%")) &&
                    (!request.IsFinish.HasValue || x.IsFinish == request.IsFinish) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate));

            return new ActivityStatusGetPagedListResponse
            {
                TotalCount = await statuses
                    .CountAsync(ct),
                LastModifyDateTime = await statuses
                    .MaxAsync(x => x != null ? x.ModifyDateTime ?? x.CreateDateTime : (DateTime?) null, ct),
                Statuses = await statuses
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
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

            var entry = await _storage.AddAsync(newStatus, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

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

            _storage.Update(oldStatus);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityStatusChange>();

            await _storage.ActivityStatuses
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.WithUpdateLog(userId, s =>
                {
                    s.IsDeleted = true;
                    s.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityStatusChange>();

            await _storage.ActivityStatuses
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.WithUpdateLog(userId, s =>
                {
                    s.IsDeleted = false;
                    s.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}

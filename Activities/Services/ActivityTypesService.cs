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
using Crm.Apps.Activities.v1.Requests;
using Crm.Apps.Activities.v1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Activities.Services
{
    public class ActivityTypesService : IActivityTypesService
    {
        private readonly ActivitiesStorage _storage;

        public ActivityTypesService(ActivitiesStorage storage)
        {
            _storage = storage;
        }

        public Task<ActivityType> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.ActivityTypes
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<ActivityType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.ActivityTypes
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<ActivityTypeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            ActivityTypeGetPagedListRequest request,
            CancellationToken ct)
        {
            var types = _storage.ActivityTypes
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate));

            return new ActivityTypeGetPagedListResponse
            {
                TotalCount = await types
                    .CountAsync(ct),
                LastModifyDateTime = await types
                    .MaxAsync(x => x.ModifyDateTime, ct),
                Types = await types
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, ActivityType type, CancellationToken ct)
        {
            var newType = new ActivityType();
            var change = newType.WithCreateLog(userId, t =>
            {
                t.Id = Guid.NewGuid();
                t.AccountId = type.AccountId;
                t.Name = type.Name;
                t.IsDeleted = type.IsDeleted;
                t.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newType, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            ActivityType oldType,
            ActivityType newType,
            CancellationToken ct)
        {
            var change = oldType.WithUpdateLog(userId, t =>
            {
                t.Name = newType.Name;
                t.IsDeleted = newType.IsDeleted;
                t.ModifyDateTime = DateTime.UtcNow;
            });

            _storage.Update(oldType);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityTypeChange>();

            await _storage.ActivityTypes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.WithUpdateLog(userId, t =>
                {
                    t.IsDeleted = true;
                    t.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityTypeChange>();

            await _storage.ActivityTypes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.WithUpdateLog(userId, t =>
                {
                    t.IsDeleted = false;
                    t.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
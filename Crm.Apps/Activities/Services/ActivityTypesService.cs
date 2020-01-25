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
    public class ActivityTypesService : IActivityTypesService
    {
        private readonly ActivitiesStorage _activitiesStorage;

        public ActivityTypesService(ActivitiesStorage activitiesStorage)
        {
            _activitiesStorage = activitiesStorage;
        }

        public Task<ActivityType> GetAsync(Guid id, CancellationToken ct)
        {
            return _activitiesStorage.ActivityTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<ActivityType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _activitiesStorage.ActivityTypes
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public Task<List<ActivityType>> GetPagedListAsync(
            ActivityTypeGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _activitiesStorage.ActivityTypes
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
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, ActivityType type, CancellationToken ct)
        {
            var newType = new ActivityType();
            var change = newType.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = type.AccountId;
                x.Name = type.Name;
                x.IsDeleted = type.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _activitiesStorage.AddAsync(newType, ct);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            ActivityType oldType,
            ActivityType newType,
            CancellationToken ct)
        {
            var change = oldType.WithUpdateLog(userId, x =>
            {
                x.Name = newType.Name;
                x.IsDeleted = newType.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
            });

            _activitiesStorage.Update(oldType);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityTypeChange>();

            await _activitiesStorage.ActivityTypes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x =>
                {
                    x.IsDeleted = true;
                    x.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityTypeChange>();

            await _activitiesStorage.ActivityTypes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x =>
                {
                    x.IsDeleted = false;
                    x.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }
    }
}
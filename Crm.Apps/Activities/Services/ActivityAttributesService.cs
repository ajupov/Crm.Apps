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
    public class ActivityAttributesService : IActivityAttributesService
    {
        private readonly ActivitiesStorage _activitiesStorage;

        public ActivityAttributesService(ActivitiesStorage activitiesStorage)
        {
            _activitiesStorage = activitiesStorage;
        }

        public Task<ActivityAttribute> GetAsync(Guid id, CancellationToken ct)
        {
            return _activitiesStorage.ActivityAttributes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<ActivityAttribute[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _activitiesStorage.ActivityAttributes
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync(ct);
        }

        public Task<ActivityAttribute[]> GetPagedListAsync(
            ActivityAttributeGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _activitiesStorage.ActivityAttributes
                .AsNoTracking()
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.Types == null || !request.Types.Any() || request.Types.Contains(x.Type)) &&
                    (request.Key.IsEmpty() || EF.Functions.Like(x.Key, $"{request.Key}%")) &&
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

        public async Task<Guid> CreateAsync(Guid userId, ActivityAttribute attribute, CancellationToken ct)
        {
            var newAttribute = new ActivityAttribute();
            var change = newAttribute.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = attribute.AccountId;
                x.Type = attribute.Type;
                x.Key = attribute.Key;
                x.IsDeleted = attribute.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _activitiesStorage.AddAsync(newAttribute, ct);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            ActivityAttribute oldAttribute,
            ActivityAttribute newAttribute,
            CancellationToken ct)
        {
            var change = oldAttribute.WithUpdateLog(userId, x =>
            {
                x.AccountId = newAttribute.AccountId;
                x.Type = newAttribute.Type;
                x.Key = newAttribute.Key;
                x.IsDeleted = newAttribute.IsDeleted;
            });

            _activitiesStorage.Update(oldAttribute);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityAttributeChange>();

            await _activitiesStorage.ActivityAttributes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityAttributeChange>();

            await _activitiesStorage.ActivityAttributes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }
    }
}
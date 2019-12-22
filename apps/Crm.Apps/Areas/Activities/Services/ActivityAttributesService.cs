using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Helpers;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;
using Crm.Apps.Areas.Activities.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Activities.Services
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
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<ActivityAttribute[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _activitiesStorage.ActivityAttributes
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync(ct);
        }

        public Task<ActivityAttribute[]> GetPagedListAsync(
            ActivityAttributeGetPagedListRequest request,
            CancellationToken ct)
        {
            return _activitiesStorage.ActivityAttributes
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.Types == null || !request.Types.Any() || request.Types.Contains(x.Type)) &&
                    (request.Key.IsEmpty() || EF.Functions.Like(x.Key, $"{request.Key}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArrayAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, ActivityAttributeCreateRequest request, CancellationToken ct)
        {
            var attribute = new ActivityAttribute();
            var change = attribute.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = request.AccountId;
                x.Type = request.Type;
                x.Key = request.Key;
                x.IsDeleted = request.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _activitiesStorage.AddAsync(attribute, ct);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            ActivityAttribute attribute,
            ActivityAttributeUpdateRequest request,
            CancellationToken ct)
        {
            var change = attribute.WithUpdateLog(userId, x =>
            {
                x.AccountId = request.AccountId;
                x.Type = request.Type;
                x.Key = request.Key;
                x.IsDeleted = request.IsDeleted;
            });

            _activitiesStorage.Update(attribute);
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
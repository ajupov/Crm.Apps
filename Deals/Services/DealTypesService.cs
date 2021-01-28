using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Deals.Helpers;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Storages;
using Crm.Apps.Deals.V1.Requests;
using Crm.Apps.Deals.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Deals.Services
{
    public class DealTypesService : IDealTypesService
    {
        private readonly DealsStorage _storage;

        public DealTypesService(DealsStorage storage)
        {
            _storage = storage;
        }

        public Task<DealType> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.DealTypes
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<DealType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.DealTypes
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<DealTypeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            DealTypeGetPagedListRequest request,
            CancellationToken ct)
        {
            var types = _storage.DealTypes
                .AsNoTracking()
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Name.IsEmpty() || EF.Functions.ILike(x.Name, $"{request.Name}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate));

            return new DealTypeGetPagedListResponse
            {
                TotalCount = await types
                    .CountAsync(ct),
                LastModifyDateTime = await types
                    .MaxAsync(x => x != null ? x.ModifyDateTime ?? x.CreateDateTime : (DateTime?) null, ct),
                Types = await types
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, DealType type, CancellationToken ct)
        {
            var newType = new DealType();
            var change = newType.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = type.AccountId;
                x.Name = type.Name;
                x.IsDeleted = type.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newType, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            DealType oldType,
            DealType newType,
            CancellationToken ct)
        {
            var change = oldType.WithUpdateLog(userId, x =>
            {
                x.Name = newType.Name;
                x.IsDeleted = newType.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
            });

            _storage.Update(oldType);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealTypeChange>();

            await _storage.DealTypes
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
            var changes = new List<DealTypeChange>();

            await _storage.DealTypes
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Deals.Helpers;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.RequestParameters;
using Crm.Apps.Deals.Storages;
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

        public Task<DealType> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.DealTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<DealType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.DealTypes
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public Task<List<DealType>> GetPagedListAsync(DealTypeGetPagedListRequestParameter request, CancellationToken ct)
        {
            return _storage.DealTypes
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

        public async Task UpdateAsync(Guid userId, DealType oldType, DealType newType,
            CancellationToken ct)
        {
            var change = oldType.WithUpdateLog(userId, x =>
            {
                x.Name = newType.Name;
                x.IsDeleted = newType.IsDeleted;
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
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealTypeChange>();

            await _storage.DealTypes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
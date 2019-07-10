using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Helpers;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Parameters;
using Crm.Apps.Deals.Storages;
using Crm.Utils.Guid;
using Crm.Utils.String;
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
            return _storage.DealTypes.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<DealType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.DealTypes.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<DealType>> GetPagedListAsync(DealTypeGetPagedListParameter parameter, CancellationToken ct)
        {
            return _storage.DealTypes.Where(x =>
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (parameter.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{parameter.Name}%")) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
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

            var entry = await _storage.AddAsync(newType, ct).ConfigureAwait(false);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

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
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealTypeChange>();

            await _storage.DealTypes.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealTypeChange>();

            await _storage.DealTypes.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}
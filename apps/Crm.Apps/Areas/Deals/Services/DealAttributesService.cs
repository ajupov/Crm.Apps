using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.String;
using Crm.Apps.Areas.Deals.Helpers;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;
using Crm.Apps.Areas.Deals.Storages;
using Crm.Apps.Utils;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Deals.Services
{
    public class DealAttributesService : IDealAttributesService
    {
        private readonly DealsStorage _storage;

        public DealAttributesService(DealsStorage storage)
        {
            _storage = storage;
        }

        public Task<DealAttribute> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.DealAttributes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<DealAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.DealAttributes
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public Task<List<DealAttribute>> GetPagedListAsync(DealAttributeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.DealAttributes
                .AsNoTracking()
                .Where(x =>
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (parameter.Types == null || !parameter.Types.Any() || parameter.Types.Contains(x.Type)) &&
                    (parameter.Key.IsEmpty() || EF.Functions.Like(x.Key, $"{parameter.Key}%")) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate) &&
                    (!parameter.MinModifyDate.HasValue || x.ModifyDateTime >= parameter.MinModifyDate) &&
                    (!parameter.MaxModifyDate.HasValue || x.ModifyDateTime <= parameter.MaxModifyDate))
                .SortBy(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, DealAttribute attribute, CancellationToken ct)
        {
            var newAttribute = new DealAttribute();
            var change = newAttribute.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = attribute.AccountId;
                x.Type = attribute.Type;
                x.Key = attribute.Key;
                x.IsDeleted = attribute.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newAttribute, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, DealAttribute oldAttribute, DealAttribute newAttribute,
            CancellationToken ct)
        {
            var change = oldAttribute.WithUpdateLog(userId, x =>
            {
                x.Type = newAttribute.Type;
                x.Key = newAttribute.Key;
                x.IsDeleted = newAttribute.IsDeleted;
            });

            _storage.Update(oldAttribute);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealAttributeChange>();

            await _storage.DealAttributes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealAttributeChange>();

            await _storage.DealAttributes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
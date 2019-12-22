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
    public class DealStatusesService : IDealStatusesService
    {
        private readonly DealsStorage _storage;

        public DealStatusesService(DealsStorage storage)
        {
            _storage = storage;
        }

        public Task<DealStatus> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.DealStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<DealStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.DealStatuses
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public Task<List<DealStatus>> GetPagedListAsync(DealStatusGetPagedListParameter parameter, CancellationToken ct)
        {
            return _storage.DealStatuses
                .AsNoTracking()
                .Where(x =>
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (parameter.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{parameter.Name}%")) &&
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

        public async Task<Guid> CreateAsync(Guid userId, DealStatus status, CancellationToken ct)
        {
            var newStatus = new DealStatus();
            var change = newStatus.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = status.AccountId;
                x.Name = status.Name;
                x.IsDeleted = status.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newStatus, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, DealStatus oldStatus, DealStatus newStatus,
            CancellationToken ct)
        {
            var change = oldStatus.WithUpdateLog(userId, x =>
            {
                x.Name = newStatus.Name;
                x.IsDeleted = newStatus.IsDeleted;
            });

            _storage.Update(oldStatus);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealStatusChange>();

            await _storage.DealStatuses
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealStatusChange>();

            await _storage.DealStatuses
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
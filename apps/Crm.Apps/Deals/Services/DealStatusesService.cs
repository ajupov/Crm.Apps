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
    public class DealStatusesService : IDealStatusesService
    {
        private readonly DealsStorage _storage;

        public DealStatusesService(DealsStorage storage)
        {
            _storage = storage;
        }

        public Task<DealStatus> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.DealStatuses.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<DealStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.DealStatuses.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<DealStatus>> GetPagedListAsync(DealStatusGetPagedListParameter parameter, CancellationToken ct)
        {
            return _storage.DealStatuses.Where(x =>
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

            var entry = await _storage.AddAsync(newStatus, ct).ConfigureAwait(false);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

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
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealStatusChange>();

            await _storage.DealStatuses.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealStatusChange>();

            await _storage.DealStatuses.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}
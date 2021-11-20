using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Stock.Helpers;
using Crm.Apps.Stock.Mappers;
using Crm.Apps.Stock.Models;
using Crm.Apps.Stock.Storages;
using Crm.Apps.Stock.V1.Requests;
using Crm.Apps.Stock.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Stock.Services
{
    public class StockConsumptionsService : IStockConsumptionsService
    {
        private readonly StockStorage _storage;

        public StockConsumptionsService(StockStorage storage)
        {
            _storage = storage;
        }

        public Task<StockConsumption> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.StockConsumptions
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .Include(x => x.Type)
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<StockConsumption>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.StockConsumptions
                .AsNoTracking()
                .Include(x => x.Type)
                .Include(x => x.Items)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<StockConsumptionGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            StockConsumptionGetPagedListRequest request,
            CancellationToken ct)
        {
            var consumptions = await _storage.StockConsumptions
                .AsNoTracking()
                .Include(x => x.Type)
                .Include(x => x.Items)
                .Where(x => x.AccountId == accountId &&
                            (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                            (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                            (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                            (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                            (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .ToListAsync(ct);

            return new StockConsumptionGetPagedListResponse
            {
                TotalCount = consumptions
                    .Count(x => x.FilterByAdditional(request)),
                LastModifyDateTime = consumptions
                    .Max(x => x.ModifyDateTime),
                Consumptions = consumptions
                    .Where(x => x.FilterByAdditional(request))
                    .AsQueryable()
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToList()
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, StockConsumption consumption, CancellationToken ct)
        {
            var newStockConsumption = new StockConsumption();

            var change = newStockConsumption.CreateWithLog(userId, x =>
            {
                x.Id = consumption.Id;
                x.AccountId = consumption.AccountId;
                x.CreateUserId = userId;
                x.Type = consumption.Type;
                x.OrderId = consumption.OrderId;
                x.IsDeleted = consumption.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.Items = consumption.Items.Map(x.Id);
            });

            var entry = await _storage.AddAsync(newStockConsumption, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            StockConsumption oldConsumption,
            StockConsumption newConsumption,
            CancellationToken ct)
        {
            var change = oldConsumption.UpdateWithLog(userId, x =>
            {
                x.AccountId = newConsumption.AccountId;
                x.Type = newConsumption.Type;
                x.OrderId = newConsumption.OrderId;
                x.IsDeleted = newConsumption.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
                x.Items = newConsumption.Items.Map(x.Id);
            });

            _storage.Update(oldConsumption);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<StockConsumptionChange>();

            await _storage.StockConsumptions
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, d =>
                {
                    d.IsDeleted = true;
                    d.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<StockConsumptionChange>();

            await _storage.StockConsumptions
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, d =>
                {
                    d.IsDeleted = false;
                    d.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}

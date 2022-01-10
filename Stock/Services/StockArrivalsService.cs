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
    public class StockArrivalsService : IStockArrivalsService
    {
        private readonly StockStorage _storage;

        public StockArrivalsService(StockStorage storage)
        {
            _storage = storage;
        }

        public Task<StockArrival> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.StockArrivals
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .Include(x => x.Items)
                .ThenInclude(x => x.Room)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<StockArrival>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.StockArrivals
                .AsNoTracking()
                .Include(x => x.Items)
                .ThenInclude(x => x.Room)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<StockArrivalGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            StockArrivalGetPagedListRequest request,
            CancellationToken ct)
        {
            var arrivals = await _storage.StockArrivals
                .AsNoTracking()
                .Include(x => x.Items)
                .ThenInclude(x => x.Room)
                .Where(x => x.AccountId == accountId &&
                            (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                            (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                            (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                            (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                            (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .ToListAsync(ct);

            return new StockArrivalGetPagedListResponse
            {
                TotalCount = arrivals
                    .Count(x => x.FilterByAdditional(request)),
                LastModifyDateTime = arrivals
                    .Max(x => x.ModifyDateTime),
                Arrivals = arrivals
                    .Where(x => x.FilterByAdditional(request))
                    .AsQueryable()
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToList()
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, StockArrival arrival, CancellationToken ct)
        {
            var newStockArrival = new StockArrival();

            var change = newStockArrival.CreateWithLog(userId, x =>
            {
                x.Id = arrival.Id;
                x.AccountId = arrival.AccountId;
                x.CreateUserId = userId;
                x.Type = arrival.Type;
                x.SupplierId = arrival.SupplierId;
                x.OrderId = arrival.OrderId;
                x.InventoryId = arrival.InventoryId;
                x.IsDeleted = arrival.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.Items = arrival.Items.Map(x.Id);
            });

            var entry = await _storage.AddAsync(newStockArrival, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            StockArrival oldArrival,
            StockArrival newArrival,
            CancellationToken ct)
        {
            var change = oldArrival.UpdateWithLog(userId, x =>
            {
                x.AccountId = newArrival.AccountId;
                x.Type = newArrival.Type;
                x.SupplierId = newArrival.SupplierId;
                x.OrderId = newArrival.OrderId;
                x.InventoryId = newArrival.InventoryId;
                x.IsDeleted = newArrival.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
                x.Items = newArrival.Items.Map(x.Id);
            });

            _storage.Update(oldArrival);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<StockArrivalChange>();

            await _storage.StockArrivals
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
            var changes = new List<StockArrivalChange>();

            await _storage.StockArrivals
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

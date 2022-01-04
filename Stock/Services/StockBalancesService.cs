using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Stock.Helpers;
using Crm.Apps.Stock.Models;
using Crm.Apps.Stock.Storages;
using Crm.Apps.Stock.V1.Requests;
using Crm.Apps.Stock.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Stock.Services
{
    public class StockBalancesService : IStockBalancesService
    {
        private readonly StockStorage _storage;

        public StockBalancesService(StockStorage storage)
        {
            _storage = storage;
        }

        public Task<StockBalance> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.StockBalances
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .Include(x => x.Room)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<StockBalance>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.StockBalances
                .AsNoTracking()
                .Include(x => x.Room)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<StockBalanceGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            StockBalanceGetPagedListRequest request,
            CancellationToken ct)
        {
            var balances = await _storage.StockBalances
                .AsNoTracking()
                .Include(x => x.Room)
                .Where(x => x.AccountId == accountId &&
                            (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                            (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                            (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                            (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                            (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .ToListAsync(ct);

            return new StockBalanceGetPagedListResponse
            {
                TotalCount = balances
                    .Count(x => x.FilterByAdditional(request)),
                LastModifyDateTime = balances
                    .Max(x => x.ModifyDateTime),
                Balances = balances
                    .Where(x => x.FilterByAdditional(request))
                    .AsQueryable()
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToList()
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, StockBalance balance, CancellationToken ct)
        {
            var newStockBalance = new StockBalance();

            var change = newStockBalance.CreateWithLog(userId, x =>
            {
                x.Id = balance.Id;
                x.AccountId = balance.AccountId;
                x.CreateUserId = userId;
                x.RoomId = balance.RoomId;
                x.ProductId = balance.ProductId;
                x.Count = balance.Count;
                x.IsDeleted = balance.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;

                // x.UniqueElementIds = balance.UniqueElementIds;
            });

            var entry = await _storage.AddAsync(newStockBalance, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            StockBalance oldBalance,
            StockBalance newBalance,
            CancellationToken ct)
        {
            var change = oldBalance.UpdateWithLog(userId, x =>
            {
                x.AccountId = newBalance.AccountId;
                x.RoomId = newBalance.RoomId;
                x.ProductId = newBalance.ProductId;
                x.Count = newBalance.Count;
                x.IsDeleted = newBalance.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;

                // x.UniqueElementIds = newBalance.UniqueElementIds;
            });

            _storage.Update(oldBalance);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<StockBalanceChange>();

            await _storage.StockBalances
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
            var changes = new List<StockBalanceChange>();

            await _storage.StockBalances
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

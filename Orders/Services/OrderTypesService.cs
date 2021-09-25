using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Orders.Helpers;
using Crm.Apps.Orders.Models;
using Crm.Apps.Orders.Storages;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Orders.Services
{
    public class OrderTypesService : IOrderTypesService
    {
        private readonly OrdersStorage _storage;

        public OrderTypesService(OrdersStorage storage)
        {
            _storage = storage;
        }

        public Task<OrderType> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.OrderTypes
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<OrderType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.OrderTypes
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<OrderTypeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            OrderTypeGetPagedListRequest request,
            CancellationToken ct)
        {
            var types = _storage.OrderTypes
                .AsNoTracking()
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Name.IsEmpty() || EF.Functions.ILike(x.Name, $"{request.Name}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate));

            return new OrderTypeGetPagedListResponse
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

        public async Task<Guid> CreateAsync(Guid userId, OrderType type, CancellationToken ct)
        {
            var newType = new OrderType();
            var change = newType.CreateWithLog(userId, x =>
            {
                x.Id = type.Id;
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
            OrderType oldType,
            OrderType newType,
            CancellationToken ct)
        {
            var change = oldType.UpdateWithLog(userId, x =>
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
            var changes = new List<OrderTypeChange>();

            await _storage.OrderTypes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, t =>
                {
                    t.IsDeleted = true;
                    t.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<OrderTypeChange>();

            await _storage.OrderTypes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, t =>
                {
                    t.IsDeleted = false;
                    t.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}

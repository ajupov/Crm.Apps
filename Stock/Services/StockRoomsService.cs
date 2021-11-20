using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Stock.Helpers;
using Crm.Apps.Stock.Models;
using Crm.Apps.Stock.Storages;
using Crm.Apps.Stock.V1.Requests;
using Crm.Apps.Stock.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Stock.Services
{
    public class StockRoomsService : IStockRoomsService
    {
        private readonly StockStorage _storage;

        public StockRoomsService(StockStorage storage)
        {
            _storage = storage;
        }

        public Task<StockRoom> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.StockRooms
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<StockRoom>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.StockRooms
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<StockRoomGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            StockRoomGetPagedListRequest request,
            CancellationToken ct)
        {
            var rooms = _storage.StockRooms
                .AsNoTracking()
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Name.IsEmpty() || EF.Functions.ILike(x.Name, $"{request.Name}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate));

            return new StockRoomGetPagedListResponse
            {
                TotalCount = await rooms
                    .CountAsync(ct),
                LastModifyDateTime = await rooms
                    .MaxAsync(x => x != null ? x.ModifyDateTime ?? x.CreateDateTime : (DateTime?)null, ct),
                Rooms = await rooms
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, StockRoom room, CancellationToken ct)
        {
            var newRoom = new StockRoom();
            var change = newRoom.CreateWithLog(userId, x =>
            {
                x.Id = room.Id;
                x.AccountId = room.AccountId;
                x.Name = room.Name;
                x.IsDeleted = room.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newRoom, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            StockRoom oldRoom,
            StockRoom newRoom,
            CancellationToken ct)
        {
            var change = oldRoom.UpdateWithLog(userId, x =>
            {
                x.Name = newRoom.Name;
                x.IsDeleted = newRoom.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
            });

            _storage.Update(oldRoom);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<StockRoomChange>();

            await _storage.StockRooms
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
            var changes = new List<StockRoomChange>();

            await _storage.StockRooms
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Decimal;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Orders.Helpers;
using Crm.Apps.Orders.Mappers;
using Crm.Apps.Orders.Models;
using Crm.Apps.Orders.Storages;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Orders.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly OrdersStorage _storage;

        public OrdersService(OrdersStorage storage)
        {
            _storage = storage;
        }

        public Task<Order> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.Orders
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .Include(x => x.Type)
                .Include(x => x.Status)
                .Include(x => x.Items)
                .Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Order>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Orders
                .AsNoTracking()
                .Include(x => x.Type)
                .Include(x => x.Status)
                .Include(x => x.Items)
                .Include(x => x.AttributeLinks)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<OrderGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            OrderGetPagedListRequest request,
            CancellationToken ct)
        {
            var orders = await _storage.Orders
                .AsNoTracking()
                .Include(x => x.Type)
                .Include(x => x.Status)
                .Include(x => x.Items)
                .Include(x => x.AttributeLinks)
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Name.IsEmpty() || EF.Functions.ILike(x.Name, $"{request.Name}%")) &&
                    (!request.MinStartDateTime.HasValue || x.StartDateTime >= request.MinStartDateTime) &&
                    (!request.MaxStartDateTime.HasValue || x.StartDateTime <= request.MaxStartDateTime) &&
                    (!request.MinEndDateTime.HasValue || x.EndDateTime >= request.MinEndDateTime) &&
                    (!request.MaxEndDateTime.HasValue || x.EndDateTime <= request.MaxEndDateTime) &&
                    (request.MinSum.IsEmpty() || x.Sum >= request.MinSum) &&
                    (request.MaxSum.IsEmpty() || x.Sum <= request.MaxSum) &&
                    (request.MinSumWithoutDiscount.IsEmpty() ||
                     x.SumWithoutDiscount >= request.MinSumWithoutDiscount) &&
                    (request.MaxSumWithoutDiscount.IsEmpty() ||
                     x.SumWithoutDiscount <= request.MaxSumWithoutDiscount) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .ToListAsync(ct);

            return new OrderGetPagedListResponse
            {
                TotalCount = orders
                    .Count(x => x.FilterByAdditional(request)),
                LastModifyDateTime = orders
                    .Max(x => x.ModifyDateTime),
                Orders = orders
                    .Where(x => x.FilterByAdditional(request))
                    .AsQueryable()
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToList()
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, Order order, CancellationToken ct)
        {
            var newOrder = new Order();
            var type = await _storage.OrderTypes.FirstAsync(t => t.Id == order.TypeId, ct);
            var status = await _storage.OrderStatuses.FirstAsync(t => t.Id == order.StatusId, ct);

            var change = newOrder.CreateWithLog(userId, x =>
            {
                x.Id = order.Id;
                x.AccountId = order.AccountId;
                x.TypeId = order.TypeId;
                x.StatusId = order.StatusId;
                x.CreateUserId = userId;
                x.ResponsibleUserId = order.ResponsibleUserId;
                x.CustomerId = order.CustomerId;
                x.Name = order.Name;
                x.StartDateTime = order.StartDateTime;
                x.EndDateTime = order.EndDateTime;
                x.Sum = order.Sum;
                x.SumWithoutDiscount = order.SumWithoutDiscount;
                x.IsDeleted = order.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.Type = type;
                x.Status = status;
                x.AttributeLinks = order.AttributeLinks.Map(x.Id);
                x.Items = order.Items.Map(x.Id);
            });

            var entry = await _storage.AddAsync(newOrder, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Order oldOrder, Order newOrder, CancellationToken ct)
        {
            var change = oldOrder.UpdateWithLog(userId, x =>
            {
                x.AccountId = newOrder.AccountId;
                x.TypeId = newOrder.TypeId;
                x.StatusId = newOrder.StatusId;
                x.ResponsibleUserId = newOrder.ResponsibleUserId;
                x.CustomerId = newOrder.CustomerId;
                x.Name = newOrder.Name;
                x.StartDateTime = newOrder.StartDateTime;
                x.EndDateTime = newOrder.EndDateTime;
                x.Sum = newOrder.Sum;
                x.SumWithoutDiscount = newOrder.SumWithoutDiscount;
                x.IsDeleted = newOrder.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
                x.AttributeLinks = newOrder.AttributeLinks.Map(x.Id);
                x.Items = newOrder.Items.Map(x.Id);
            });

            _storage.Update(oldOrder);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<OrderChange>();

            await _storage.Orders
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
            var changes = new List<OrderChange>();

            await _storage.Orders
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

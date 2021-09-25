using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Customers.Helpers;
using Crm.Apps.Customers.Models;
using Crm.Apps.Customers.Storages;
using Crm.Apps.Customers.V1.Requests;
using Crm.Apps.Customers.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Customers.Services
{
    public class CustomerAttributesService : ICustomerAttributesService
    {
        private readonly CustomersStorage _storage;

        public CustomerAttributesService(CustomersStorage storage)
        {
            _storage = storage;
        }

        public Task<CustomerAttribute> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.CustomerAttributes
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<CustomerAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.CustomerAttributes
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<CustomerAttributeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            CustomerAttributeGetPagedListRequest request,
            CancellationToken ct)
        {
            var attributes = _storage.CustomerAttributes
                .AsNoTracking()
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Types == null || !request.Types.Any() || request.Types.Contains(x.Type)) &&
                    (request.Key.IsEmpty() || EF.Functions.ILike(x.Key, $"{request.Key}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate));

            return new CustomerAttributeGetPagedListResponse
            {
                TotalCount = await attributes
                    .CountAsync(ct),
                LastModifyDateTime = await attributes
                    .MaxAsync(x => x != null ? x.ModifyDateTime ?? x.CreateDateTime : (DateTime?)null, ct),
                Attributes = await attributes
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, CustomerAttribute attribute, CancellationToken ct)
        {
            var newAttribute = new CustomerAttribute();
            var change = newAttribute.CreateWithLog(userId, x =>
            {
                x.Id = attribute.Id;
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

        public async Task UpdateAsync(
            Guid userId,
            CustomerAttribute oldAttribute,
            CustomerAttribute newAttribute,
            CancellationToken ct)
        {
            var change = oldAttribute.UpdateWithLog(userId, x =>
            {
                x.Type = newAttribute.Type;
                x.Key = newAttribute.Key;
                x.IsDeleted = newAttribute.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
            });

            _storage.Update(oldAttribute);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<CustomerAttributeChange>();

            await _storage.CustomerAttributes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, a =>
                {
                    a.IsDeleted = true;
                    a.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<CustomerAttributeChange>();

            await _storage.CustomerAttributes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, a =>
                {
                    a.IsDeleted = false;
                    a.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}

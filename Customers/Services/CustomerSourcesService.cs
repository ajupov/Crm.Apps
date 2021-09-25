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
    public class CustomerSourcesService : ICustomerSourcesService
    {
        private readonly CustomersStorage _storage;

        public CustomerSourcesService(CustomersStorage storage)
        {
            _storage = storage;
        }

        public Task<CustomerSource> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.CustomerSources
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<CustomerSource>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.CustomerSources
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public async Task<CustomerSourceGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            CustomerSourceGetPagedListRequest request,
            CancellationToken ct)
        {
            var sources = _storage.CustomerSources
                .AsNoTracking()
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Name.IsEmpty() || EF.Functions.ILike(x.Name, $"{request.Name}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate));

            return new CustomerSourceGetPagedListResponse
            {
                TotalCount = await sources
                    .CountAsync(ct),
                LastModifyDateTime = await sources
                    .MaxAsync(x => x != null ? x.ModifyDateTime ?? x.CreateDateTime : (DateTime?)null, ct),
                Sources = await sources
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, CustomerSource source, CancellationToken ct)
        {
            var newSource = new CustomerSource();
            var change = newSource.CreateWithLog(userId, x =>
            {
                x.Id = source.Id;
                x.AccountId = source.AccountId;
                x.Name = source.Name;
                x.IsDeleted = source.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newSource, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            CustomerSource oldSource,
            CustomerSource newSource,
            CancellationToken ct)
        {
            var change = oldSource.UpdateWithLog(userId, x =>
            {
                x.Name = newSource.Name;
                x.IsDeleted = newSource.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
            });

            _storage.Update(oldSource);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<CustomerSourceChange>();

            await _storage.CustomerSources
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, s =>
                {
                    s.IsDeleted = true;
                    s.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<CustomerSourceChange>();

            await _storage.CustomerSources
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, s =>
                {
                    s.IsDeleted = false;
                    s.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}

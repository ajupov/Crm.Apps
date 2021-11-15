using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Suppliers.Helpers;
using Crm.Apps.Suppliers.Mappers;
using Crm.Apps.Suppliers.Models;
using Crm.Apps.Suppliers.Storages;
using Crm.Apps.Suppliers.V1.Requests;
using Crm.Apps.Suppliers.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Suppliers.Services
{
    public class SuppliersService : ISuppliersService
    {
        private readonly SuppliersStorage _storage;

        public SuppliersService(SuppliersStorage storage)
        {
            _storage = storage;
        }

        public Task<Supplier> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.Suppliers
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Supplier>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Suppliers
                .AsNoTracking()
                .Include(x => x.AttributeLinks)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<SupplierGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            SupplierGetPagedListRequest request,
            CancellationToken ct)
        {
            var suppliers = await _storage.Suppliers
                .AsNoTracking()
                .Include(x => x.AttributeLinks)
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Name.IsEmpty() || EF.Functions.ILike(x.Name, $"{request.Name}%")) &&
                    (request.Phone.IsEmpty() || x.Phone == request.Phone) &&
                    (request.Email.IsEmpty() || x.Email == request.Email) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .ToListAsync(ct);

            return new SupplierGetPagedListResponse
            {
                TotalCount = suppliers
                    .Count(x => x.FilterByAdditional(request)),
                LastModifyDateTime = suppliers
                    .Max(x => x.ModifyDateTime),
                Suppliers = suppliers
                    .Where(x => x.FilterByAdditional(request))
                    .AsQueryable()
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToList()
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, Supplier supplier, CancellationToken ct)
        {
            var newSupplier = new Supplier();

            var change = newSupplier.CreateWithLog(userId, x =>
            {
                x.Id = supplier.Id;
                x.AccountId = supplier.AccountId;
                x.CreateUserId = userId;
                x.Name = supplier.Name;
                x.Phone = supplier.Phone;
                x.Email = supplier.Email;
                x.IsDeleted = supplier.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.AttributeLinks = supplier.AttributeLinks.Map(x.Id);
            });

            var entry = await _storage.AddAsync(newSupplier, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Supplier oldSupplier, Supplier newSupplier, CancellationToken ct)
        {
            var change = oldSupplier.UpdateWithLog(userId, x =>
            {
                x.AccountId = newSupplier.AccountId;
                x.Name = newSupplier.Name;
                x.Phone = newSupplier.Phone;
                x.Email = newSupplier.Email;
                x.IsDeleted = newSupplier.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
                x.AttributeLinks = newSupplier.AttributeLinks.Map(x.Id);
            });

            _storage.Update(oldSupplier);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<SupplierChange>();

            await _storage.Suppliers
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, l =>
                {
                    l.IsDeleted = true;
                    l.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<SupplierChange>();

            await _storage.Suppliers
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, l =>
                {
                    l.IsDeleted = false;
                    l.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}

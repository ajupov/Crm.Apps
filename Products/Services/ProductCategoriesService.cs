using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Products.Helpers;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.Storages;
using Crm.Apps.Products.v1.Requests;
using Crm.Apps.Products.v1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Products.Services
{
    public class ProductCategoriesService : IProductCategoriesService
    {
        private readonly ProductsStorage _storage;

        public ProductCategoriesService(ProductsStorage storage)
        {
            _storage = storage;
        }

        public Task<ProductCategory> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.ProductCategories
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<ProductCategory>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.ProductCategories
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<ProductCategoryGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            ProductCategoryGetPagedListRequest request,
            CancellationToken ct)
        {
            var categories = _storage.ProductCategories
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Name.IsEmpty() || EF.Functions.ILike(x.Name, $"{request.Name}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate));

            return new ProductCategoryGetPagedListResponse
            {
                TotalCount = await categories
                    .CountAsync(ct),
                LastModifyDateTime = await categories
                    .MaxAsync(x => x.ModifyDateTime, ct),
                Categories = await categories
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, ProductCategory category, CancellationToken ct)
        {
            var newCategory = new ProductCategory();
            var change = newCategory.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = category.AccountId;
                x.Name = category.Name;
                x.IsDeleted = category.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newCategory, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            ProductCategory oldCategory,
            ProductCategory newCategory,
            CancellationToken ct)
        {
            var change = oldCategory.WithUpdateLog(userId, x =>
            {
                x.Name = newCategory.Name;
                x.IsDeleted = newCategory.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
            });

            _storage.Update(oldCategory);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ProductCategoryChange>();

            await _storage.ProductCategories
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.WithUpdateLog(userId, c =>
                {
                    c.IsDeleted = true;
                    c.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ProductCategoryChange>();

            await _storage.ProductCategories
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.WithUpdateLog(userId, c =>
                {
                    c.IsDeleted = false;
                    c.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
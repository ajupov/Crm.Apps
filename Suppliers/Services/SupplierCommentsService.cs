using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Suppliers.Models;
using Crm.Apps.Suppliers.Storages;
using Crm.Apps.Suppliers.V1.Requests;
using Crm.Apps.Suppliers.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Suppliers.Services
{
    public class SupplierCommentsService : ISupplierCommentsService
    {
        private readonly SuppliersStorage _storage;

        public SupplierCommentsService(SuppliersStorage storage)
        {
            _storage = storage;
        }

        public async Task<SupplierCommentGetPagedListResponse> GetPagedListAsync(
            SupplierCommentGetPagedListRequest request,
            CancellationToken ct)
        {
            var queryable = _storage.SupplierComments
                .AsNoTracking()
                .Where(x =>
                    x.SupplierId == request.SupplierId &&
                    (!request.BeforeCreateDateTime.HasValue || x.CreateDateTime < request.BeforeCreateDateTime) &&
                    (!request.AfterCreateDateTime.HasValue || x.CreateDateTime > request.AfterCreateDateTime));

            var minCreateDateTime = _storage.SupplierComments
                .AsNoTracking()
                .Where(x => x.SupplierId == request.SupplierId)
                .Min(x => x != null ? x.CreateDateTime : (DateTime?)null);

            var comments = await queryable
                .SortBy(request.SortBy, request.OrderBy)
                .Take(request.Limit)
                .ToListAsync(ct);

            return new SupplierCommentGetPagedListResponse
            {
                HasCommentsBefore = comments.Any() && minCreateDateTime < comments.Min(x => x.CreateDateTime),
                Comments = comments
            };
        }

        public async Task CreateAsync(Guid userId, SupplierComment comment, CancellationToken ct)
        {
            var newComment = new SupplierComment
            {
                Id = Guid.NewGuid(),
                SupplierId = comment.SupplierId,
                CommentatorUserId = userId,
                Value = comment.Value,
                CreateDateTime = DateTime.UtcNow
            };

            await _storage.AddAsync(newComment, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}

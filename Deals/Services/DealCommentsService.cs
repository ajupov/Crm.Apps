using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Storages;
using Crm.Apps.Deals.v1.Requests;
using Crm.Apps.Deals.v1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Deals.Services
{
    public class DealCommentsService : IDealCommentsService
    {
        private readonly DealsStorage _storage;

        public DealCommentsService(DealsStorage storage)
        {
            _storage = storage;
        }

        public async Task<DealCommentGetPagedListResponse> GetPagedListAsync(
            DealCommentGetPagedListRequest request,
            CancellationToken ct)
        {
            var comments = _storage.DealComments
                .Where(x =>
                    x.DealId == request.DealId &&
                    (request.Value.IsEmpty() || EF.Functions.Like(x.Value, $"{request.Value}%")) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new DealCommentGetPagedListResponse
            {
                TotalCount = await comments
                    .CountAsync(ct),
                Comments = await comments
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }

        public async Task CreateAsync(Guid userId, DealComment comment, CancellationToken ct)
        {
            var newComment = new DealComment
            {
                Id = Guid.NewGuid(),
                DealId = comment.DealId,
                CommentatorUserId = userId,
                Value = comment.Value,
                CreateDateTime = DateTime.UtcNow
            };

            await _storage.AddAsync(newComment, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
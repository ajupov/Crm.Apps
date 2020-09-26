using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Storages;
using Crm.Apps.Deals.V1.Requests;
using Crm.Apps.Deals.V1.Responses;
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
            var queryable = _storage.DealComments
                .Where(x =>
                    x.DealId == request.DealId &&
                    (!request.BeforeCreateDateTime.HasValue || x.CreateDateTime < request.BeforeCreateDateTime) &&
                    (!request.AfterCreateDateTime.HasValue || x.CreateDateTime > request.AfterCreateDateTime));

            var minCreateDateTime = _storage.DealComments
                .Where(x => x.DealId == request.DealId)
                .Min(x => x != null ? x.CreateDateTime : (DateTime?) null);

            var comments = await queryable
                .SortBy(request.SortBy, request.OrderBy)
                .Take(request.Limit)
                .ToListAsync(ct);

            return new DealCommentGetPagedListResponse
            {
                HasCommentsBefore = comments.Any() && minCreateDateTime < comments.Min(x => x.CreateDateTime),
                Comments = comments
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

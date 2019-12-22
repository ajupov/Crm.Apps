using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.String;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.RequestParameters;
using Crm.Apps.Areas.Deals.Storages;
using Crm.Apps.Utils;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Deals.Services
{
    public class DealCommentsService : IDealCommentsService
    {
        private readonly DealsStorage _storage;

        public DealCommentsService(DealsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<DealComment>> GetPagedListAsync(DealCommentGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _storage.DealComments
                .AsNoTracking()
                .Where(x =>
                    x.DealId == request.DealId &&
                    (request.CommentatorUserId.IsEmpty() || x.CommentatorUserId == request.CommentatorUserId) &&
                    (request.Value.IsEmpty() || EF.Functions.Like(x.Value, $"{request.Value}%")) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.String;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;
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

        public Task<List<DealComment>> GetPagedListAsync(DealCommentGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.DealComments
                .AsNoTracking()
                .Where(x =>
                    x.DealId == parameter.DealId &&
                    (parameter.CommentatorUserId.IsEmpty() || x.CommentatorUserId == parameter.CommentatorUserId) &&
                    (parameter.Value.IsEmpty() || EF.Functions.Like(x.Value, $"{parameter.Value}%")) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .SortBy(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
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
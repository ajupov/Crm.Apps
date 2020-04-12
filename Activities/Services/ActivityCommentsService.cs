using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Storages;
using Crm.Apps.Activities.v1.Requests;
using Crm.Apps.Activities.v1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Activities.Services
{
    public class ActivityCommentsService : IActivityCommentsService
    {
        private readonly ActivitiesStorage _storage;

        public ActivityCommentsService(ActivitiesStorage storage)
        {
            _storage = storage;
        }

        public async Task<ActivityCommentGetPagedListResponse> GetPagedListAsync(
            ActivityCommentGetPagedListRequest request,
            CancellationToken ct)
        {
            var comments = _storage.ActivityComments
                .Where(x =>
                    x.ActivityId == request.ActivityId &&
                    (request.Value.IsEmpty() || EF.Functions.ILike(x.Value, $"{request.Value}%")) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new ActivityCommentGetPagedListResponse
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

        public async Task CreateAsync(Guid userId, ActivityComment comment, CancellationToken ct)
        {
            var newComment = new ActivityComment
            {
                Id = Guid.NewGuid(),
                ActivityId = comment.ActivityId,
                CommentatorUserId = userId,
                Value = comment.Value,
                CreateDateTime = DateTime.UtcNow
            };

            await _storage.AddAsync(newComment, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Storages;
using Crm.Apps.Activities.V1.Requests;
using Crm.Apps.Activities.V1.Responses;
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
                    (!request.BeforeCreateDateTime.HasValue || x.CreateDateTime <= request.BeforeCreateDateTime) &&
                    (!request.AfterCreateDateTime.HasValue || x.CreateDateTime > request.AfterCreateDateTime));

            return new ActivityCommentGetPagedListResponse
            {
                Comments = await comments
                    .SortBy(request.SortBy, request.OrderBy)
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

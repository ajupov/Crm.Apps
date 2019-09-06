using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Storages;
using Crm.Utils.Guid;
using Crm.Utils.Sorting;
using Crm.Utils.String;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Activities.Services
{
    public class ActivityCommentsService : IActivityCommentsService
    {
        private readonly ActivitiesStorage _activitiesStorage;

        public ActivityCommentsService(ActivitiesStorage activitiesStorage)
        {
            _activitiesStorage = activitiesStorage;
        }

        public Task<ActivityComment[]> GetPagedListAsync(
            ActivityCommentGetPagedListRequest request,
            CancellationToken ct)
        {
            return _activitiesStorage.ActivityComments
                .Where(x =>
                    x.ActivityId == request.ActivityId &&
                    (request.CommentatorUserId.IsEmpty() || x.CommentatorUserId == request.CommentatorUserId) &&
                    (request.Value.IsEmpty() || EF.Functions.Like(x.Value, $"{request.Value}%")) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArrayAsync(ct);
        }

        public async Task CreateAsync(Guid userId, ActivityCommentCreateRequest request, CancellationToken ct)
        {
            var newComment = new ActivityComment
            {
                Id = Guid.NewGuid(),
                ActivityId = request.ActivityId,
                CommentatorUserId = userId,
                Value = request.Value,
                CreateDateTime = DateTime.UtcNow
            };

            await _activitiesStorage.AddAsync(newComment, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }
    }
}
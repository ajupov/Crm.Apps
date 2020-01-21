using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Activities.Storages;
using Crm.Apps.Activities.v1.Models;
using Crm.Apps.Activities.v1.RequestParameters;
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

        public Task<List<ActivityComment>> GetPagedListAsync(
            ActivityCommentGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _activitiesStorage.ActivityComments
                .AsNoTracking()
                .Where(x =>
                    x.ActivityId == request.ActivityId &&
                    (request.CommentatorUserId.IsEmpty() || x.CommentatorUserId == request.CommentatorUserId) &&
                    (request.Value.IsEmpty() || EF.Functions.Like(x.Value, $"{request.Value}%")) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
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

            await _activitiesStorage.AddAsync(newComment, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Tasks.Models;
using Crm.Apps.Tasks.Storages;
using Crm.Apps.Tasks.V1.Requests;
using Crm.Apps.Tasks.V1.Responses;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Crm.Apps.Tasks.Services
{
    public class TaskCommentsService : ITaskCommentsService
    {
        private readonly TasksStorage _storage;

        public TaskCommentsService(TasksStorage storage)
        {
            _storage = storage;
        }

        public async Task<TaskCommentGetPagedListResponse> GetPagedListAsync(
            TaskCommentGetPagedListRequest request,
            CancellationToken ct)
        {
            var queryable = _storage.TaskComments
                .AsNoTracking()
                .Where(x =>
                    x.TaskId == request.TaskId &&
                    (!request.BeforeCreateDateTime.HasValue || x.CreateDateTime < request.BeforeCreateDateTime) &&
                    (!request.AfterCreateDateTime.HasValue || x.CreateDateTime > request.AfterCreateDateTime));

            var minCreateDateTime = _storage.TaskComments
                .AsNoTracking()
                .Where(x => x.TaskId == request.TaskId)
                .Min(x => x != null ? x.CreateDateTime : (DateTime?) null);

            var comments = await queryable
                .SortBy(request.SortBy, request.OrderBy)
                .Take(request.Limit)
                .ToListAsync(ct);

            return new TaskCommentGetPagedListResponse
            {
                HasCommentsBefore = comments.Any() && minCreateDateTime < comments.Min(x => x.CreateDateTime),
                Comments = comments
            };
        }

        public async Task CreateAsync(Guid userId, TaskComment comment, CancellationToken ct)
        {
            var newComment = new TaskComment
            {
                Id = Guid.NewGuid(),
                TaskId = comment.TaskId,
                CommentatorUserId = userId,
                Value = comment.Value,
                CreateDateTime = DateTime.UtcNow
            };

            await _storage.AddAsync(newComment, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}

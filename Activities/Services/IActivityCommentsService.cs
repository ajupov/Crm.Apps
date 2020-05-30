using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.V1.Requests;
using Crm.Apps.Activities.V1.Responses;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityCommentsService
    {
        Task<ActivityCommentGetPagedListResponse> GetPagedListAsync(
            ActivityCommentGetPagedListRequest request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, ActivityComment comment, CancellationToken ct);
    }
}

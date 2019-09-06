using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityCommentsService
    {
        Task<ActivityComment[]> GetPagedListAsync(ActivityCommentGetPagedListRequest request, CancellationToken ct);

        Task CreateAsync(Guid userId, ActivityCommentCreateRequest request, CancellationToken ct);
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;

namespace Crm.Apps.Areas.Activities.Services
{
    public interface IActivityCommentsService
    {
        Task<ActivityComment[]> GetPagedListAsync(ActivityCommentGetPagedListRequest request, CancellationToken ct);

        Task CreateAsync(Guid userId, ActivityCommentCreateRequest request, CancellationToken ct);
    }
}
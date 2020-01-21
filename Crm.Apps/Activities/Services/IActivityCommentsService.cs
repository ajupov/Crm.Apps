using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.v1.Models;
using Crm.Apps.Activities.v1.RequestParameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityCommentsService
    {
        Task<List<ActivityComment>> GetPagedListAsync(
            ActivityCommentGetPagedListRequestParameter request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, ActivityComment comment, CancellationToken ct);
    }
}
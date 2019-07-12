using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Parameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityCommentsService
    {
        Task<List<ActivityComment>> GetPagedListAsync(ActivityCommentGetPagedListParameter parameter, CancellationToken ct);

        Task CreateAsync(Guid userId, ActivityComment comment, CancellationToken ct);
    }
}
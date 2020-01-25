using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.v1.Models;
using Crm.Apps.Deals.v1.RequestParameters;

namespace Crm.Apps.Deals.Services
{
    public interface IDealCommentsService
    {
        Task<List<DealComment>> GetPagedListAsync(
            DealCommentGetPagedListRequestParameter request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, DealComment comment, CancellationToken ct);
    }
}
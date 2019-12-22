using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.RequestParameters;

namespace Crm.Apps.Areas.Deals.Services
{
    public interface IDealCommentsService
    {
        Task<List<DealComment>> GetPagedListAsync(DealCommentGetPagedListRequestParameter request, CancellationToken ct);

        Task CreateAsync(Guid userId, DealComment comment, CancellationToken ct);
    }
}
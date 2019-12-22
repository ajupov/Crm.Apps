using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;

namespace Crm.Apps.Areas.Deals.Services
{
    public interface IDealCommentsService
    {
        Task<List<DealComment>> GetPagedListAsync(DealCommentGetPagedListParameter parameter, CancellationToken ct);

        Task CreateAsync(Guid userId, DealComment comment, CancellationToken ct);
    }
}